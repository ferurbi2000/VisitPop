using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoVisita;
using VisitPop.Application.Interfaces.TipoVisita;
using VisitPop.Application.Validation.TipoVisita;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController()]
    [Route("api/TipoVisitas")]
    [ApiVersion("1.0")]
    public class TipoVisitasController : Controller
    {
        private readonly ITipoVisitaRepository _tipoVisitaRepository;
        private readonly IMapper _mapper;

        public TipoVisitasController(ITipoVisitaRepository tipoVisitaRepository
           , IMapper mapper)
        {
            _tipoVisitaRepository = tipoVisitaRepository ??
                throw new ArgumentNullException(nameof(tipoVisitaRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTipoVisitas")]
        public async Task<IActionResult> GetTipoVisitas([FromQuery] TipoVisitaParametersDto tipoVisitaParametersDto)
        {
            var tipoVisitasFromRepo = await _tipoVisitaRepository.GetTipoVisitasAsync(tipoVisitaParametersDto);

            var paginationMetadata = new
            {
                totalCount = tipoVisitasFromRepo.TotalCount,
                pageSize = tipoVisitasFromRepo.PageSize,
                currentPageSize = tipoVisitasFromRepo.CurrentPageSize,
                currentStartIndex = tipoVisitasFromRepo.CurrentStartIndex,
                currentEndIndex = tipoVisitasFromRepo.CurrentEndIndex,
                pageNumber = tipoVisitasFromRepo.PageNumber,
                totalPages = tipoVisitasFromRepo.TotalPages,
                hasPrevious = tipoVisitasFromRepo.HasPrevious,
                hasNext = tipoVisitasFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tipoVisitasDto = _mapper.Map<IEnumerable<TipoVisitaDto>>(tipoVisitasFromRepo);
            var response = new Response<IEnumerable<TipoVisitaDto>>(tipoVisitasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetTipoVisita")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TipoVisitaDto>> GetTipoVisita(int id)
        {
            var tipoVisitaFromRepo = await _tipoVisitaRepository.GetTipoVisitaAsync(id);

            if (tipoVisitaFromRepo == null)
            {
                return NotFound();
            }

            var tipoVisitaDto = _mapper.Map<TipoVisitaDto>(tipoVisitaFromRepo);
            var response = new Response<TipoVisitaDto>(tipoVisitaDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TipoVisitaDto>> AddTipoVisita([FromBody] TipoVisitaForCreationDto tipoVisitaForCreation)
        {
            var validationResults = new TipoVisitaForCreationDtoValidator().Validate(tipoVisitaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var tipoVisita = _mapper.Map<TipoVisita>(tipoVisitaForCreation);
            await _tipoVisitaRepository.AddTipoVisita(tipoVisita);
            var saveSuccessful = await _tipoVisitaRepository.SaveAsync();

            if (saveSuccessful)
            {
                TipoVisita tipoVisitaFromRepo = await _tipoVisitaRepository.GetTipoVisitaAsync(tipoVisita.Id);
                var tipoVisitaDto = _mapper.Map<TipoVisitaDto>(tipoVisitaFromRepo);
                var response = new Response<TipoVisitaDto>(tipoVisitaDto);

                return CreatedAtRoute("GetTipoVisita",
                    new { tipoVisitaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteTipoVisita(int id)
        {
            var tipoVisitaFromRepo = await _tipoVisitaRepository.GetTipoVisitaAsync(id);

            if (tipoVisitaFromRepo == null)
            {
                return NotFound();
            }

            _tipoVisitaRepository.DeleteTipoVisita(tipoVisitaFromRepo);
            await _tipoVisitaRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTipoVisita(int id, TipoVisitaForUpdateDto tipoVisita)
        {
            var tipoVisitaFromRepo = await _tipoVisitaRepository.GetTipoVisitaAsync(id);

            if (tipoVisitaFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TipoVisitaForUpdateDtoValidator().Validate(tipoVisita);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(tipoVisita, tipoVisitaFromRepo);
            _tipoVisitaRepository.UpdateTipoVisita(tipoVisitaFromRepo);

            await _tipoVisitaRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateTipoVisita(int id, JsonPatchDocument<TipoVisitaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTipoVisita = await _tipoVisitaRepository.GetTipoVisitaAsync(id);

            if (existingTipoVisita == null)
            {
                return NotFound();
            }

            var tipoVisitaToPatch = _mapper.Map<TipoVisitaForUpdateDto>(existingTipoVisita); // map the tipoVisita we got from the database to an updatable tipoVisita model
            patchDoc.ApplyTo(tipoVisitaToPatch, ModelState); // apply patchdoc updates to the updatable tipoVisita

            if (!TryValidateModel(tipoVisitaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(tipoVisitaToPatch, existingTipoVisita); // apply updates from the updatable tipoVisita to the db entity so we can apply the updates to the database
            _tipoVisitaRepository.UpdateTipoVisita(existingTipoVisita); // apply business updates to data if needed

            await _tipoVisitaRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
