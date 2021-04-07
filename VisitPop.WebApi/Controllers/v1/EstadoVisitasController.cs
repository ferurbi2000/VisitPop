using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EstadoVisita;
using VisitPop.Application.Interfaces.EstadoVisita;
using VisitPop.Application.Validation.EstadoVisita;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/EstadoVisitas")]
    [ApiVersion("1.0")]
    public class EstadoVisitasController : Controller
    {
        private readonly IEstadoVisitaRepository _estadoVisitaRepository;
        private readonly IMapper _mapper;

        public EstadoVisitasController(IEstadoVisitaRepository repo,
            IMapper map)
        {
            _estadoVisitaRepository = repo
                ?? throw new ArgumentNullException(nameof(repo));
            _mapper = map
                ?? throw new ArgumentNullException(nameof(map));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetEstadoVisitas")]
        public async Task<IActionResult> GetEstadoVisitas([FromQuery] EstadoVisitaParametersDto estadoVisitaParametersDto)
        {
            var estadoVisitasFromRepo = await _estadoVisitaRepository.GetEstadoVisitasAsync(estadoVisitaParametersDto);

            var paginationMetadata = new
            {
                totalCount = estadoVisitasFromRepo.TotalCount,
                pageSize = estadoVisitasFromRepo.PageSize,
                currentPageSize = estadoVisitasFromRepo.CurrentPageSize,
                currentStartIndex = estadoVisitasFromRepo.CurrentStartIndex,
                currentEndIndex = estadoVisitasFromRepo.CurrentEndIndex,
                pageNumber = estadoVisitasFromRepo.PageNumber,
                totalPages = estadoVisitasFromRepo.TotalPages,
                hasPrevious = estadoVisitasFromRepo.HasPrevious,
                hasNext = estadoVisitasFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var estadoVisitasDto = _mapper.Map<IEnumerable<EstadoVisitaDto>>(estadoVisitasFromRepo);
            var response = new Response<IEnumerable<EstadoVisitaDto>>(estadoVisitasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetEstadoVisita")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EstadoVisitaDto>> GetEstadoVisita(int Id)
        {
            var estadoVisitaFromRepo = await _estadoVisitaRepository.GetEstadoVisitaAsync(Id);

            if (estadoVisitaFromRepo == null)
            {
                return NotFound();
            }

            var estadoVisitaDto = _mapper.Map<EstadoVisitaDto>(estadoVisitaFromRepo);
            var response = new Response<EstadoVisitaDto>(estadoVisitaDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EstadoVisitaDto>> AddEstadoVisita([FromBody] EstadoVisitaForCreationDto estadoVisitaForCreation)
        {
            var validationResults = new EstadoVisitaForCreationDtoValidator().Validate(estadoVisitaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var estadoVisita = _mapper.Map<EstadoVisita>(estadoVisitaForCreation);
            await _estadoVisitaRepository.AddEstadoVisita(estadoVisita);
            var saveSuccessful = await _estadoVisitaRepository.SaveAsync();

            if (saveSuccessful)
            {
                var estadoVisitaFromRepo = await _estadoVisitaRepository.GetEstadoVisitaAsync(estadoVisita.Id);
                var estadoVisitaDto = _mapper.Map<EstadoVisitaDto>(estadoVisitaFromRepo);
                var response = new Response<EstadoVisitaDto>(estadoVisitaDto);

                return CreatedAtRoute("GetEstadoVisita",
                    new { estadoVisitaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEstadoVisita(int Id)
        {
            var estadoVisitaFromRepo = await _estadoVisitaRepository.GetEstadoVisitaAsync(Id);

            if (estadoVisitaFromRepo == null)
            {
                return NotFound();
            }

            _estadoVisitaRepository.DeleteEstadoVisita(estadoVisitaFromRepo);
            await _estadoVisitaRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateEstadoVisita(int Id, EstadoVisitaForUpdateDto estadoVisita)
        {
            var estadoVisitaFromRepo = await _estadoVisitaRepository.GetEstadoVisitaAsync(Id);

            if (estadoVisitaFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new EstadoVisitaForUpdateDtoValidator().Validate((estadoVisita));
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(estadoVisita, estadoVisitaFromRepo);
            _estadoVisitaRepository.UpdateEstadoVisita(estadoVisitaFromRepo);

            await _estadoVisitaRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateEstadoVisita(int Id, JsonPatchDocument<EstadoVisitaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingEstadoVisita = await _estadoVisitaRepository.GetEstadoVisitaAsync(Id);

            if (existingEstadoVisita == null)
            {
                return NotFound();
            }

            var estadoVisitaToPatch = _mapper.Map<EstadoVisitaForUpdateDto>(existingEstadoVisita); // map the estadoVisita we got from the database to an updatable estadoVisita model
            patchDoc.ApplyTo(estadoVisitaToPatch, ModelState); // apply patchdoc updates to the updatable estadoVisita

            if (!TryValidateModel(estadoVisitaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(estadoVisitaToPatch, existingEstadoVisita); // apply updates from the updatable estadoVisita to the db entity so we can apply the updates to the database
            _estadoVisitaRepository.UpdateEstadoVisita(existingEstadoVisita); // apply business updates to data if needed

            await _estadoVisitaRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
