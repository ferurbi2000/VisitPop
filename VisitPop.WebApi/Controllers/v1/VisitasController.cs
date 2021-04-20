using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Visita;
using VisitPop.Application.Interfaces.Visita;
using VisitPop.Application.Validation.Visita;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Visitas")]
    [ApiVersion("1.0")]
    public class VisitasController : Controller
    {
        private readonly IVisitaRepository _visitaRepo;
        private readonly IMapper _mapper;

        public VisitasController(IVisitaRepository repo,
            IMapper mapper)
        {
            _visitaRepo = repo
                ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVisitas")]
        public async Task<IActionResult> GetVisitas([FromQuery] VisitaParametersDto visitaParameters)
        {
            var visitasFromRepo = await _visitaRepo.GetVisitasAsync(visitaParameters);

            var paginationMetadata = new
            {
                totalCount = visitasFromRepo.MetaData.TotalCount,
                pageSize = visitasFromRepo.MetaData.PageSize,
                currentPageSize = visitasFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = visitasFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = visitasFromRepo.MetaData.CurrentEndIndex,
                pageNumber = visitasFromRepo.MetaData.PageNumber,
                totalPages = visitasFromRepo.MetaData.TotalPages,
                hasPrevious = visitasFromRepo.MetaData.HasPrevious,
                hasNext = visitasFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var visitasDto = _mapper.Map<IEnumerable<VisitaDto>>(visitasFromRepo);
            var response = new Response<IEnumerable<VisitaDto>>(visitasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetVisita")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitaDto>> GetVisita(int id)
        {
            var visitaFromRepo = await _visitaRepo.GetVisitaAsync(id);

            if (visitaFromRepo == null)
            {
                return NotFound();
            }

            var visitaDto = _mapper.Map<VisitaDto>(visitaFromRepo);
            var response = new Response<VisitaDto>(visitaDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitaDto>> AddVisita([FromBody] VisitaForCreationDto visitaForCreation)
        {
            var validationResults = new VisitaForCreationDtoValidator().Validate(visitaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var visita = _mapper.Map<Visita>(visitaForCreation);
            await _visitaRepo.AddVisita(visita);
            var saveSuccessful = await _visitaRepo.SaveAsync();

            if (saveSuccessful)
            {
                var visitaFromRepo = await _visitaRepo.GetVisitaAsync(visita.Id);
                var visitaDto = _mapper.Map<VisitaDto>(visitaFromRepo);
                var response = new Response<VisitaDto>(visitaDto);

                return CreatedAtRoute("GetVisita",
                    new { visitaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVisita(int id)
        {
            var visitaFromRepo = await _visitaRepo.GetVisitaAsync(id);

            if (visitaFromRepo == null)
            {
                return NotFound();
            }

            _visitaRepo.DeleteVisita(visitaFromRepo);
            await _visitaRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVisita(int id, VisitaForUpdateDto visita)
        {
            var visitaFromRepo = await _visitaRepo.GetVisitaAsync(id);

            if (visitaFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new VisitaForUpdateDtoValidator().Validate(visita);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(visita, visitaFromRepo);
            _visitaRepo.UpdateVisita(visitaFromRepo);

            await _visitaRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVisita(int id, JsonPatchDocument<VisitaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingVisita = await _visitaRepo.GetVisitaAsync(id);

            if (existingVisita == null)
            {
                return NotFound();
            }

            var visitaToPatch = _mapper.Map<VisitaForUpdateDto>(existingVisita); // map the visita we got from the database to an updatable visita model
            patchDoc.ApplyTo(visitaToPatch, ModelState); // apply patchdoc updates to the updatable visita

            if (!TryValidateModel(visitaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(visitaToPatch, existingVisita); // apply updates from the updatable visita to the db entity so we can apply the updates to the database
            _visitaRepo.UpdateVisita(existingVisita); // apply business updates to data if needed

            await _visitaRepo.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
