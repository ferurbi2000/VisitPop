using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Application.Interfaces.Visit;
using VisitPop.Application.Validation.Visit;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Visits")]
    [ApiVersion("1.0")]
    public class VisitsController : Controller
    {
        private readonly IVisitRepository _visitRepo;
        private readonly IMapper _mapper;

        public VisitsController(IVisitRepository repo,
            IMapper mapper)
        {
            _visitRepo = repo
                ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVisits")]
        public async Task<IActionResult> GetVisits([FromQuery] VisitParametersDto visitParameters)
        {
            var visitsFromRepo = await _visitRepo.GetVisitsAsync(visitParameters);

            var paginationMetadata = new
            {
                totalCount = visitsFromRepo.MetaData.TotalCount,
                pageSize = visitsFromRepo.MetaData.PageSize,
                currentPageSize = visitsFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = visitsFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = visitsFromRepo.MetaData.CurrentEndIndex,
                pageNumber = visitsFromRepo.MetaData.PageNumber,
                totalPages = visitsFromRepo.MetaData.TotalPages,
                hasPrevious = visitsFromRepo.MetaData.HasPrevious,
                hasNext = visitsFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var visitsDto = _mapper.Map<IEnumerable<VisitDto>>(visitsFromRepo);
            var response = new Response<IEnumerable<VisitDto>>(visitsDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetVisit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitDto>> GetVisit(int id)
        {
            var visitFromRepo = await _visitRepo.GetVisitAsync(id);

            if (visitFromRepo == null)
            {
                return NotFound();
            }

            var visitDto = _mapper.Map<VisitDto>(visitFromRepo);
            var response = new Response<VisitDto>(visitDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitDto>> AddVisit([FromBody] VisitForCreationDto visitForCreation)
        {
            var validationResults = new VisitForCreationDtoValidator().Validate(visitForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var visit = _mapper.Map<Visit>(visitForCreation);
            await _visitRepo.AddVisit(visit);
            var saveSuccessful = await _visitRepo.SaveAsync();

            if (saveSuccessful)
            {
                var visitFromRepo = await _visitRepo.GetVisitAsync(visit.Id);
                var visitDto = _mapper.Map<VisitDto>(visitFromRepo);
                var response = new Response<VisitDto>(visitDto);

                return CreatedAtRoute("GetVisit",
                    new { visitDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVisit(int id)
        {
            var visitFromRepo = await _visitRepo.GetVisitAsync(id);

            if (visitFromRepo == null)
            {
                return NotFound();
            }

            _visitRepo.DeleteVisit(visitFromRepo);
            await _visitRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVisit(int id, VisitForUpdateDto visit)
        {
            var visitFromRepo = await _visitRepo.GetVisitAsync(id);

            if (visitFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new VisitForUpdateDtoValidator().Validate(visit);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(visit, visitFromRepo);
            _visitRepo.UpdateVisit(visitFromRepo);

            await _visitRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVisit(int id, JsonPatchDocument<VisitForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingVisit = await _visitRepo.GetVisitAsync(id);

            if (existingVisit == null)
            {
                return NotFound();
            }

            var visitToPatch = _mapper.Map<VisitForUpdateDto>(existingVisit); // map the visita we got from the database to an updatable visita model
            patchDoc.ApplyTo(visitToPatch, ModelState); // apply patchdoc updates to the updatable visita

            if (!TryValidateModel(visitToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(visitToPatch, existingVisit); // apply updates from the updatable visita to the db entity so we can apply the updates to the database
            _visitRepo.UpdateVisit(existingVisit); // apply business updates to data if needed

            await _visitRepo.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
