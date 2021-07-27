using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitPerson;
using VisitPop.Application.Interfaces.VisitPerson;
using VisitPop.Application.Validation.VisitPerson;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/VisitPersons")]
    [ApiVersion("1.0")]
    public class VisitPersonsController : Controller
    {
        private readonly IVisitPersonRepository _visitPersonRepository;
        private readonly IMapper _mapper;

        public VisitPersonsController(IVisitPersonRepository visitPersonRepository
            , IMapper mapper)
        {
            _visitPersonRepository = visitPersonRepository ??
                throw new ArgumentNullException(nameof(visitPersonRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVisitPersons")]
        public async Task<IActionResult> GetVisitPersons([FromQuery] VisitPersonParametersDto visitPersonParametersDto)
        {
            var visitPersonsFromRepo = await _visitPersonRepository.GetVisitPersonsAsync(visitPersonParametersDto);

            var paginationMetadata = new
            {
                totalCount = visitPersonsFromRepo.MetaData.TotalCount,
                pageSize = visitPersonsFromRepo.MetaData.PageSize,
                currentPageSize = visitPersonsFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = visitPersonsFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = visitPersonsFromRepo.MetaData.CurrentEndIndex,
                pageNumber = visitPersonsFromRepo.MetaData.PageNumber,
                totalPages = visitPersonsFromRepo.MetaData.TotalPages,
                hasPrevious = visitPersonsFromRepo.MetaData.HasPrevious,
                hasNext = visitPersonsFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var visitPersonsDto = _mapper.Map<IEnumerable<VisitPersonDto>>(visitPersonsFromRepo);
            var response = new Response<IEnumerable<VisitPersonDto>>(visitPersonsDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetVisitPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitPersonDto>> GetVisitPerson(int id)
        {
            var visitPersonFromRepo = await _visitPersonRepository.GetVisitPersonAsync(id);

            if (visitPersonFromRepo == null)
            {
                return NotFound();
            }

            var visitPersonDto = _mapper.Map<VisitPersonDto>(visitPersonFromRepo);
            var response = new Response<VisitPersonDto>(visitPersonDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitPersonDto>> AddVisitPerson([FromBody] VisitPersonForCreationDto visitPersonForCreation)
        {
            var validationResults = new VisitPersonForCreationDtoValidator().Validate(visitPersonForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var visitPerson = _mapper.Map<VisitPerson>(visitPersonForCreation);
            await _visitPersonRepository.AddVisitPerson(visitPerson);
            var saveSuccessful = await _visitPersonRepository.SaveAsync();

            if (saveSuccessful)
            {
                var visitPersonFromRepo = await _visitPersonRepository.GetVisitPersonAsync(visitPerson.Id);
                var visitPersonDto = _mapper.Map<VisitPersonDto>(visitPersonFromRepo);
                var response = new Response<VisitPersonDto>(visitPersonDto);

                return CreatedAtRoute("GetVisitPerson",
                    new { visitPersonDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVisitPerson(int id)
        {
            var visitPersonFromRepo = await _visitPersonRepository.GetVisitPersonAsync(id);

            if (visitPersonFromRepo == null)
            {
                return NotFound();
            }

            _visitPersonRepository.DeleteVisitPerson(visitPersonFromRepo);
            await _visitPersonRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVisitPerson(int id, VisitPersonForUpdateDto visitPerson)
        {
            var visitPersonFromRepo = await _visitPersonRepository.GetVisitPersonAsync(id);

            if (visitPersonFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new VisitPersonForUpdateDtoValidator().Validate(visitPerson);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(visitPerson, visitPersonFromRepo);
            _visitPersonRepository.UpdateVisitPerson(visitPersonFromRepo);

            await _visitPersonRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVisitPerson(int id, JsonPatchDocument<VisitPersonForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingVisitPerson = await _visitPersonRepository.GetVisitPersonAsync(id);

            if (existingVisitPerson == null)
            {
                return NotFound();
            }

            var visitPersonToPatch = _mapper.Map<VisitPersonForUpdateDto>(existingVisitPerson); // map the visitaPersona we got from the database to an updatable visitaPersona model
            patchDoc.ApplyTo(visitPersonToPatch, ModelState); // apply patchdoc updates to the updatable visitaPersona

            if (!TryValidateModel(visitPersonToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(visitPersonToPatch, existingVisitPerson); // apply updates from the updatable visitaPersona to the db entity so we can apply the updates to the database
            _visitPersonRepository.UpdateVisitPerson(existingVisitPerson); // apply business updates to data if needed

            await _visitPersonRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
