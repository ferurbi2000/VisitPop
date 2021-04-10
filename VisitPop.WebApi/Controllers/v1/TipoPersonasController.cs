using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPop.Application.Interfaces.TipoPersona;
using VisitPop.Application.Validation.TipoPersona;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/TipoPersonas")]
    [ApiVersion("1.0")]
    public class TipoPersonasController : Controller
    {
        private readonly ITipoPersonaRepository _tipoPersonaRepository;
        private readonly IMapper _mapper;

        public TipoPersonasController(ITipoPersonaRepository repo,
            IMapper mapper)
        {
            _tipoPersonaRepository = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTipoPersonas")]
        public async Task<IActionResult> GetTipoPersonas([FromQuery] TipoPersonaParametersDto tipoPersonaParametersDto)
        {
            var tipoPersonaFromRepo = await _tipoPersonaRepository.GetTipoPersonas(tipoPersonaParametersDto);

            var paginationMetadata = new
            {
                totalCount = tipoPersonaFromRepo.TotalCount,
                pageSize = tipoPersonaFromRepo.PageSize,
                currentPageSize = tipoPersonaFromRepo.CurrentPageSize,
                currentStartIndex = tipoPersonaFromRepo.CurrentStartIndex,
                currentEndIndex = tipoPersonaFromRepo.CurrentEndIndex,
                pageNumber = tipoPersonaFromRepo.PageNumber,
                totalPages = tipoPersonaFromRepo.TotalPages,
                hasPrevious = tipoPersonaFromRepo.HasPrevious,
                hasNext = tipoPersonaFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tipoPersonasDto = _mapper.Map<IEnumerable<TipoPersonaDto>>(tipoPersonaFromRepo);
            var response = new Response<IEnumerable<TipoPersonaDto>>(tipoPersonasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetTipoPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TipoPersonaDto>> GetTipoPersona(int id)
        {
            var tipoPersonaFromRepo = await _tipoPersonaRepository.GetTipoPersonaAsync(id);

            if (tipoPersonaFromRepo == null)
            {
                return NotFound();
            }

            var tipoPersonaDto = _mapper.Map<TipoPersonaDto>(tipoPersonaFromRepo);
            var response = new Response<TipoPersonaDto>(tipoPersonaDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TipoPersonaDto>> AddTipoPersona([FromBody] TipoPersonaForCreationDto tipoPersonaForCreation)
        {
            var validationResults = new TipoPersonaForCreationDtoValidator().Validate(tipoPersonaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var tipoPersona = _mapper.Map<TipoPersona>(tipoPersonaForCreation);
            await _tipoPersonaRepository.AddTipoPersona(tipoPersona);
            var saveSuccessful = await _tipoPersonaRepository.SaveAsync();

            if (saveSuccessful)
            {
                var tipoPersonaFromRepo = await _tipoPersonaRepository.GetTipoPersonaAsync(tipoPersona.Id);
                var tipoPersonaDto = _mapper.Map<TipoPersonaDto>(tipoPersonaFromRepo);
                var response = new Response<TipoPersonaDto>(tipoPersonaDto);

                return CreatedAtRoute("GetTipoPersona",
                    new { tipoPersonaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteTipoPersona(int id)
        {
            var tipoPersonaFromRepo = await _tipoPersonaRepository.GetTipoPersonaAsync(id);

            if (tipoPersonaFromRepo == null)
            {
                return NotFound();
            }

            _tipoPersonaRepository.DeleteTipoPersona(tipoPersonaFromRepo);
            await _tipoPersonaRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTipoPersona(int id, TipoPersonaForUpdatedDto tipoPersona)
        {
            var tipoPersonaFromRepo = await _tipoPersonaRepository.GetTipoPersonaAsync(id);

            if (tipoPersona == null)
            {
                return NotFound();
            }

            var validationResults = new TipoPersonaForUpdateDtoValidator().Validate(tipoPersona);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(tipoPersona, tipoPersonaFromRepo);
            _tipoPersonaRepository.UpdateTipoPersona(tipoPersonaFromRepo);

            await _tipoPersonaRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]

        public async Task<IActionResult> PartiallyUpdateTipoPersona(int id, JsonPatchDocument<TipoPersonaForUpdatedDto> pathDoc)
        {
            if (pathDoc == null)
            {
                return BadRequest();
            }

            var existingTipoPersona = await _tipoPersonaRepository.GetTipoPersonaAsync(id);

            if (existingTipoPersona == null)
            {
                return NotFound();
            }

            // map the tipoPersona we got from the database to an updatable tipoPersona model
            var tipoPersonaToPatch = _mapper.Map<TipoPersonaForUpdatedDto>(existingTipoPersona);
            // apply patchdoc updates to the updatable tipoPersona
            pathDoc.ApplyTo(tipoPersonaToPatch, ModelState);

            if (!TryValidateModel(tipoPersonaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable tipoPersona to the db entity so we can apply the updates to the database
            _mapper.Map(tipoPersonaToPatch, existingTipoPersona);
            // apply business updates to data if needed
            _tipoPersonaRepository.UpdateTipoPersona(existingTipoPersona);

            // save changes in the database
            await _tipoPersonaRepository.SaveAsync();

            return NoContent();
        }
    }
}
