using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitaPersona;
using VisitPop.Application.Interfaces.VisitaPersona;
using VisitPop.Application.Validation.VisitaPersona;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/VisitaPersonas")]
    [ApiVersion("1.0")]
    public class VisitaPersonasController : Controller
    {
        private readonly IVisitaPersonaRepository _visitaPersonaRepository;
        private readonly IMapper _mapper;

        public VisitaPersonasController(IVisitaPersonaRepository visitaPersonaRepository
            , IMapper mapper)
        {
            _visitaPersonaRepository = visitaPersonaRepository ??
                throw new ArgumentNullException(nameof(visitaPersonaRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVisitaPersonas")]
        public async Task<IActionResult> GetVisitaPersonas([FromQuery] VisitaPersonaParametersDto visitaPersonaParametersDto)
        {
            var visitaPersonasFromRepo = await _visitaPersonaRepository.GetVisitaPersonasAsync(visitaPersonaParametersDto);

            var paginationMetadata = new
            {
                totalCount = visitaPersonasFromRepo.MetaData.TotalCount,
                pageSize = visitaPersonasFromRepo.MetaData.PageSize,
                currentPageSize = visitaPersonasFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = visitaPersonasFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = visitaPersonasFromRepo.MetaData.CurrentEndIndex,
                pageNumber = visitaPersonasFromRepo.MetaData.PageNumber,
                totalPages = visitaPersonasFromRepo.MetaData.TotalPages,
                hasPrevious = visitaPersonasFromRepo.MetaData.HasPrevious,
                hasNext = visitaPersonasFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var visitaPersonasDto = _mapper.Map<IEnumerable<VisitaPersonaDto>>(visitaPersonasFromRepo);
            var response = new Response<IEnumerable<VisitaPersonaDto>>(visitaPersonasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetVisitaPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitaPersonaDto>> GetVisitaPersona(int id)
        {
            var visitaPersonaFromRepo = await _visitaPersonaRepository.GetVisitaPersonaAsync(id);

            if (visitaPersonaFromRepo == null)
            {
                return NotFound();
            }

            var visitaPersonaDto = _mapper.Map<VisitaPersonaDto>(visitaPersonaFromRepo);
            var response = new Response<VisitaPersonaDto>(visitaPersonaDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitaPersonaDto>> AddVisitaPersona([FromBody] VisitaPersonaForCreationDto visitaPersonaForCreation)
        {
            var validationResults = new VisitaPersonaForCreationDtoValidator().Validate(visitaPersonaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var visitaPersona = _mapper.Map<VisitaPersona>(visitaPersonaForCreation);
            await _visitaPersonaRepository.AddVisitaPersona(visitaPersona);
            var saveSuccessful = await _visitaPersonaRepository.SaveAsync();

            if (saveSuccessful)
            {
                var visitaPersonaFromRepo = await _visitaPersonaRepository.GetVisitaPersonaAsync(visitaPersona.Id);
                var visitaPersonaDto = _mapper.Map<VisitaPersonaDto>(visitaPersonaFromRepo);
                var response = new Response<VisitaPersonaDto>(visitaPersonaDto);

                return CreatedAtRoute("GetVisitaPersona",
                    new { visitaPersonaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVisitaPersona(int id)
        {
            var visitaPersonaFromRepo = await _visitaPersonaRepository.GetVisitaPersonaAsync(id);

            if (visitaPersonaFromRepo == null)
            {
                return NotFound();
            }

            _visitaPersonaRepository.DeleteVisitaPersona(visitaPersonaFromRepo);
            await _visitaPersonaRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVisitaPersona(int id, VisitaPersonaForUpdateDto visitaPersona)
        {
            var visitaPersonaFromRepo = await _visitaPersonaRepository.GetVisitaPersonaAsync(id);

            if (visitaPersonaFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new VisitaPersonaForUpdateDtoValidator().Validate(visitaPersona);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(visitaPersona, visitaPersonaFromRepo);
            _visitaPersonaRepository.UpdateVisitaPersona(visitaPersonaFromRepo);

            await _visitaPersonaRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVisitaPersona(int id, JsonPatchDocument<VisitaPersonaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingVisitaPersona = await _visitaPersonaRepository.GetVisitaPersonaAsync(id);

            if (existingVisitaPersona == null)
            {
                return NotFound();
            }

            var visitaPersonaToPatch = _mapper.Map<VisitaPersonaForUpdateDto>(existingVisitaPersona); // map the visitaPersona we got from the database to an updatable visitaPersona model
            patchDoc.ApplyTo(visitaPersonaToPatch, ModelState); // apply patchdoc updates to the updatable visitaPersona

            if (!TryValidateModel(visitaPersonaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(visitaPersonaToPatch, existingVisitaPersona); // apply updates from the updatable visitaPersona to the db entity so we can apply the updates to the database
            _visitaPersonaRepository.UpdateVisitaPersona(existingVisitaPersona); // apply business updates to data if needed

            await _visitaPersonaRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
