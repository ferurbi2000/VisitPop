using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Empresa;
using VisitPop.Application.Interfaces.Empresa;
using VisitPop.Application.Validation.Empresa;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Empresas")]
    [ApiVersion("1.0")]
    public class EmpresasController : Controller
    {
        private readonly IEmpresaRepository _empresaRepo;
        private readonly IMapper _mapper;

        public EmpresasController(IEmpresaRepository empresaRepo,
            IMapper mapper)
        {
            _empresaRepo = empresaRepo
                ?? throw new ArgumentNullException(nameof(empresaRepo));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetEmpresas")]
        public async Task<IActionResult> GetEmpresas([FromQuery] EmpresaParametersDto empresaParametersDto)
        {
            var empresasFromRepo = await _empresaRepo.GetEmpresasAsync(empresaParametersDto);

            var paginationMetadata = new
            {
                totalCount = empresasFromRepo.TotalCount,
                pageSize = empresasFromRepo.PageSize,
                currentPageSize = empresasFromRepo.CurrentPageSize,
                currentStartIndex = empresasFromRepo.CurrentStartIndex,
                currentEndIndex = empresasFromRepo.CurrentEndIndex,
                pageNumber = empresasFromRepo.PageNumber,
                totalPages = empresasFromRepo.TotalPages,
                hasPrevious = empresasFromRepo.HasPrevious,
                hasNext = empresasFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var empresasDto = _mapper.Map<IEnumerable<EmpresaDto>>(empresasFromRepo);
            var response = new Response<IEnumerable<EmpresaDto>>(empresasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmpresaDto>> GetEmpresa(int Id)
        {
            var empresaFromRepo = await _empresaRepo.GetEmpresaAsync(Id);
            if (empresaFromRepo == null)
            {
                return NotFound();
            }

            var empresaDto = _mapper.Map<EmpresaDto>(empresaFromRepo);
            var response = new Response<EmpresaDto>(empresaDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmpresaDto>> AddEmpresa([FromBody] EmpresaForCreationDto empresaForCretion)
        {
            var validationResults = new EmpresaForCreationDtoValidator().Validate(empresaForCretion);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var empresa = _mapper.Map<Empresa>(empresaForCretion);
            await _empresaRepo.AddEmpresa(empresa);
            var saveSuccessful = await _empresaRepo.SaveAsync();

            if (saveSuccessful)
            {
                var empresaFromRepo = await _empresaRepo.GetEmpresaAsync(empresa.Id);
                var empresaDto = _mapper.Map<EmpresaDto>(empresaFromRepo);
                var response = new Response<EmpresaDto>(empresaDto);

                return CreatedAtRoute("GetEmpresa",
                    new { empresaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEmpresa(int Id)
        {
            var empresaFromRepo = await _empresaRepo.GetEmpresaAsync(Id);

            if (empresaFromRepo == null)
            {
                return NotFound();
            }

            _empresaRepo.DeleteEmpresa(empresaFromRepo);
            await _empresaRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateEmpresa(int Id, EmpresaForUpdateDto empresa)
        {
            var empresaFromRepo = await _empresaRepo.GetEmpresaAsync(Id);

            if (empresaFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new EmpresaForUpdateDtoValidator().Validate(empresa);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(empresa, empresaFromRepo);
            _empresaRepo.UpdateEmpresa(empresaFromRepo);

            await _empresaRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateEmpresa(int Id, JsonPatchDocument<EmpresaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingEmpresa = await _empresaRepo.GetEmpresaAsync(Id);

            if (existingEmpresa == null)
            {
                return NotFound();
            }

            var empresaToPatch = _mapper.Map<EmpresaForUpdateDto>(existingEmpresa); // map the empresa we got from the database to an updatable empresa model
            patchDoc.ApplyTo(empresaToPatch, ModelState); // apply patchdoc updates to the updatable empresa

            if (!TryValidateModel(empresaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(empresaToPatch, existingEmpresa); // apply updates from the updatable empresa to the db entity so we can apply the updates to the database
            _empresaRepo.UpdateEmpresa(existingEmpresa); // apply business updates to data if needed

            await _empresaRepo.SaveAsync(); // save changes in the database

            return NoContent();
        }
       
    }
}
