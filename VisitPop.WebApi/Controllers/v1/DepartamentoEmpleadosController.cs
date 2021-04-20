using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.Application.Interfaces.DepartamentoEmpleado;
using VisitPop.Application.Validation.DepartamentoEmpleado;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/DepartamentoEmpleados")]
    [ApiVersion("1.0")]
    public class DepartamentoEmpleadosController : Controller
    {
        private readonly IDepartamentoEmpleadoRepository _DepartamentoEmpleadoRepository;
        private readonly IMapper _mapper;

        public DepartamentoEmpleadosController(IDepartamentoEmpleadoRepository departamentoEmpleadoRepository
            , IMapper mapper)
        {
            _DepartamentoEmpleadoRepository = departamentoEmpleadoRepository ??
                throw new ArgumentNullException(nameof(departamentoEmpleadoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetDepartamentoEmpleados")]
        public async Task<IActionResult> GetDepartamentoEmpleados([FromQuery] DepartamentoEmpleadoParametersDto DepartamentoEmpleadoParametersDto)
        {
            var DepartamentoEmpleadosFromRepo = await _DepartamentoEmpleadoRepository.GetDepartamentoEmpleadosAsync(DepartamentoEmpleadoParametersDto);

            var paginationMetadata = new
            {
                totalCount = DepartamentoEmpleadosFromRepo.MetaData.TotalCount,
                pageSize = DepartamentoEmpleadosFromRepo.MetaData.PageSize,
                currentPageSize = DepartamentoEmpleadosFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = DepartamentoEmpleadosFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = DepartamentoEmpleadosFromRepo.MetaData.CurrentEndIndex,
                pageNumber = DepartamentoEmpleadosFromRepo.MetaData.PageNumber,
                totalPages = DepartamentoEmpleadosFromRepo.MetaData.TotalPages,
                hasPrevious = DepartamentoEmpleadosFromRepo.MetaData.HasPrevious,
                hasNext = DepartamentoEmpleadosFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var DepartamentoEmpleadosDto = _mapper.Map<IEnumerable<DepartamentoEmpleadoDto>>(DepartamentoEmpleadosFromRepo);
            var response = new Response<IEnumerable<DepartamentoEmpleadoDto>>(DepartamentoEmpleadosDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetDepartamentoEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DepartamentoEmpleadoDto>> GetDepartamentoEmpleado(int Id)
        {
            var DepartamentoEmpleadoFromRepo = await _DepartamentoEmpleadoRepository.GetDepartamentoEmpleadoAsync(Id);

            if (DepartamentoEmpleadoFromRepo == null)
            {
                return NotFound();
            }

            var DepartamentoEmpleadoDto = _mapper.Map<DepartamentoEmpleadoDto>(DepartamentoEmpleadoFromRepo);
            var response = new Response<DepartamentoEmpleadoDto>(DepartamentoEmpleadoDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<DepartamentoEmpleadoDto>> AddDepartamentoEmpleado([FromBody] DepartamentoEmpleadoForCreationDto DepartamentoEmpleadoForCreation)
        {
            var validationResults = new DepartamentoEmpleadoForCreationDtoValidator().Validate(DepartamentoEmpleadoForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var DepartamentoEmpleado = _mapper.Map<DepartamentoEmpleado>(DepartamentoEmpleadoForCreation);
            await _DepartamentoEmpleadoRepository.AddDepartamentoEmpleado(DepartamentoEmpleado);
            var saveSuccessful = await _DepartamentoEmpleadoRepository.SaveAsync();

            if (saveSuccessful)
            {
                var DepartamentoEmpleadoFromRepo = await _DepartamentoEmpleadoRepository.GetDepartamentoEmpleadoAsync(DepartamentoEmpleado.Id);
                var DepartamentoEmpleadoDto = _mapper.Map<DepartamentoEmpleadoDto>(DepartamentoEmpleadoFromRepo);
                var response = new Response<DepartamentoEmpleadoDto>(DepartamentoEmpleadoDto);

                return CreatedAtRoute("GetDepartamentoEmpleado",
                    new { DepartamentoEmpleado.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDepartamentoEmpleado(int Id)
        {
            var DepartamentoEmpleadoFromRepo = await _DepartamentoEmpleadoRepository.GetDepartamentoEmpleadoAsync(Id);

            if (DepartamentoEmpleadoFromRepo == null)
            {
                return NotFound();
            }

            _DepartamentoEmpleadoRepository.DeleteDepartamentoEmpleado(DepartamentoEmpleadoFromRepo);
            await _DepartamentoEmpleadoRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateDepartamentoEmpleado(int Id, DepartamentoEmpleadoForUpdateDto DepartamentoEmpleado)
        {
            var DepartamentoEmpleadoFromRepo = await _DepartamentoEmpleadoRepository.GetDepartamentoEmpleadoAsync(Id);

            if (DepartamentoEmpleadoFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new DepartamentoEmpleadoForUpdateDtoValidator().Validate(DepartamentoEmpleado);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(DepartamentoEmpleado, DepartamentoEmpleadoFromRepo);
            _DepartamentoEmpleadoRepository.UpdateDepartamentoEmpleado(DepartamentoEmpleadoFromRepo);

            await _DepartamentoEmpleadoRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateDepartamentoEmpleado(int Id, JsonPatchDocument<DepartamentoEmpleadoForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingDepartamentoEmpleado = await _DepartamentoEmpleadoRepository.GetDepartamentoEmpleadoAsync(Id);

            if (existingDepartamentoEmpleado == null)
            {
                return NotFound();
            }

            // map the DepartamentoEmpleado we got from the database to an updatable DepartamentoEmpleado model
            var DepartamentoEmpleadoToPatch = _mapper.Map<DepartamentoEmpleadoForUpdateDto>(existingDepartamentoEmpleado);
            // apply patchdoc updates to the updatable DepartamentoEmpleado
            patchDoc.ApplyTo(DepartamentoEmpleadoToPatch, ModelState);


            if (!TryValidateModel(DepartamentoEmpleadoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable DepartamentoEmpleado to the db entity so we can apply the updates to the database
            _mapper.Map(DepartamentoEmpleadoToPatch, existingDepartamentoEmpleado);
            // apply business updates to data if needed
            _DepartamentoEmpleadoRepository.UpdateDepartamentoEmpleado(existingDepartamentoEmpleado);

            // save changes in the database
            await _DepartamentoEmpleadoRepository.SaveAsync();

            return NoContent();
        }
    }
}
