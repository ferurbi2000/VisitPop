using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Interfaces.VisitState;
using VisitPop.Application.Validation.VisitState;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/VisitStates")]
    [ApiVersion("1.0")]
    public class VisitStatesController : Controller
    {
        private readonly IVisitStateRepository _visitStateRepository;
        private readonly IMapper _mapper;

        public VisitStatesController(IVisitStateRepository repo,
            IMapper map)
        {
            _visitStateRepository = repo
                ?? throw new ArgumentNullException(nameof(repo));
            _mapper = map
                ?? throw new ArgumentNullException(nameof(map));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVisitStates")]
        public async Task<IActionResult> GetVisitStates([FromQuery] VisitStateParametersDto visitStateParametersDto)
        {
            var visitStatesFromRepo = await _visitStateRepository.GetVisitStatesAsync(visitStateParametersDto);

            var paginationMetadata = new
            {
                totalCount = visitStatesFromRepo.MetaData.TotalCount,
                pageSize = visitStatesFromRepo.MetaData.PageSize,
                currentPageSize = visitStatesFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = visitStatesFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = visitStatesFromRepo.MetaData.CurrentEndIndex,
                pageNumber = visitStatesFromRepo.MetaData.PageNumber,
                totalPages = visitStatesFromRepo.MetaData.TotalPages,
                hasPrevious = visitStatesFromRepo.MetaData.HasPrevious,
                hasNext = visitStatesFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var stateVisitsDto = _mapper.Map<IEnumerable<VisitStateDto>>(visitStatesFromRepo);
            var response = new Response<IEnumerable<VisitStateDto>>(stateVisitsDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetVisitState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitStateDto>> GetVisitState(int Id)
        {
            var visitStateFromRepo = await _visitStateRepository.GetVisitStateAsync(Id);

            if (visitStateFromRepo == null)
            {
                return NotFound();
            }

            var visitStateDto = _mapper.Map<VisitStateDto>(visitStateFromRepo);
            var response = new Response<VisitStateDto>(visitStateDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitStateDto>> AddVisitState([FromBody] VisitStateForCreationDto visitStateForCreation)
        {
            var validationResults = new VisitStateForCreationDtoValidator().Validate(visitStateForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var visitState = _mapper.Map<VisitState>(visitStateForCreation);
            await _visitStateRepository.AddVisitState(visitState);
            var saveSuccessful = await _visitStateRepository.SaveAsync();

            if (saveSuccessful)
            {
                var visitStateFromRepo = await _visitStateRepository.GetVisitStateAsync(visitState.Id);
                var visitStateDto = _mapper.Map<VisitStateDto>(visitStateFromRepo);
                var response = new Response<VisitStateDto>(visitStateDto);

                return CreatedAtRoute("GetVisitState",
                    new { visitState.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVisitState(int Id)
        {
            var visitStateFromRepo = await _visitStateRepository.GetVisitStateAsync(Id);

            if (visitStateFromRepo == null)
            {
                return NotFound();
            }

            _visitStateRepository.DeleteVisitState(visitStateFromRepo);
            await _visitStateRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVisitState(int Id, VisitStateForUpdateDto visitState)
        {
            var visitStateFromRepo = await _visitStateRepository.GetVisitStateAsync(Id);

            if (visitStateFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new VisitStateForUpdateDtoValidator().Validate((visitState));
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(visitState, visitStateFromRepo);
            _visitStateRepository.UpdateVisitState(visitStateFromRepo);

            await _visitStateRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVisitState(int Id, JsonPatchDocument<VisitStateForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingVisitState = await _visitStateRepository.GetVisitStateAsync(Id);

            if (existingVisitState == null)
            {
                return NotFound();
            }

            var visitStateToPatch = _mapper.Map<VisitStateForUpdateDto>(existingVisitState); // map the estadoVisita we got from the database to an updatable estadoVisita model
            patchDoc.ApplyTo(visitStateToPatch, ModelState); // apply patchdoc updates to the updatable estadoVisita

            if (!TryValidateModel(visitStateToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(visitStateToPatch, existingVisitState); // apply updates from the updatable estadoVisita to the db entity so we can apply the updates to the database
            _visitStateRepository.UpdateVisitState(existingVisitState); // apply business updates to data if needed

            await _visitStateRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
