using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.Application.Interfaces.PersonType;
using VisitPop.Application.Validation.PersonType;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/PersonTypes")]
    [ApiVersion("1.0")]
    public class PersonTypesController : Controller
    {
        private readonly IPersonTypeRepository _personTypeRepository;
        private readonly IMapper _mapper;

        public PersonTypesController(IPersonTypeRepository repo,
            IMapper mapper)
        {
            _personTypeRepository = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetPersonTypes")]
        public async Task<IActionResult> GetPersonTypes([FromQuery] PersonTypeParametersDto personTypeParametersDto)
        {
            var personTypeFromRepo = await _personTypeRepository.GetPersonTypes(personTypeParametersDto);

            var paginationMetadata = new
            {
                totalCount = personTypeFromRepo.MetaData.TotalCount,
                pageSize = personTypeFromRepo.MetaData.PageSize,
                currentPageSize = personTypeFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = personTypeFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = personTypeFromRepo.MetaData.CurrentEndIndex,
                pageNumber = personTypeFromRepo.MetaData.PageNumber,
                totalPages = personTypeFromRepo.MetaData.TotalPages,
                hasPrevious = personTypeFromRepo.MetaData.HasPrevious,
                hasNext = personTypeFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var personTypeDto = _mapper.Map<IEnumerable<PersonTypeDto>>(personTypeFromRepo);
            var response = new Response<IEnumerable<PersonTypeDto>>(personTypeDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetPersonType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PersonTypeDto>> GetPersonType(int id)
        {
            var personTypeFromRepo = await _personTypeRepository.GetPersonTypeAsync(id);

            if (personTypeFromRepo == null)
            {
                return NotFound();
            }

            var personTypeDto = _mapper.Map<PersonTypeDto>(personTypeFromRepo);
            var response = new Response<PersonTypeDto>(personTypeDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PersonTypeDto>> AddPersonType([FromBody] PersonTypeForCreationDto personTypeForCreation)
        {
            var validationResults = new PersonTypeForCreationDtoValidator().Validate(personTypeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var personType = _mapper.Map<PersonType>(personTypeForCreation);
            await _personTypeRepository.AddPersonType(personType);
            var saveSuccessful = await _personTypeRepository.SaveAsync();

            if (saveSuccessful)
            {
                var personTypeFromRepo = await _personTypeRepository.GetPersonTypeAsync(personType.Id);
                var personTypeDto = _mapper.Map<PersonTypeDto>(personTypeFromRepo);
                var response = new Response<PersonTypeDto>(personTypeDto);

                return CreatedAtRoute("GetPersonType",
                    new { personTypeDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeletePersonType(int id)
        {
            var personTypeFromRepo = await _personTypeRepository.GetPersonTypeAsync(id);

            if (personTypeFromRepo == null)
            {
                return NotFound();
            }

            _personTypeRepository.DeletePersonType(personTypeFromRepo);
            await _personTypeRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePersonType(int id, PersonTypeForUpdatedDto personType)
        {
            var personTypeFromRepo = await _personTypeRepository.GetPersonTypeAsync(id);

            if (personType == null)
            {
                return NotFound();
            }

            var validationResults = new PersonTypeForUpdateDtoValidator().Validate(personType);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(personType, personTypeFromRepo);
            _personTypeRepository.UpdatePersonType(personTypeFromRepo);

            await _personTypeRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdatePersonType(int id, JsonPatchDocument<PersonTypeForUpdatedDto> pathDoc)
        {
            if (pathDoc == null)
            {
                return BadRequest();
            }

            var existingPersonType = await _personTypeRepository.GetPersonTypeAsync(id);

            if (existingPersonType == null)
            {
                return NotFound();
            }

            // map the tipoPersona we got from the database to an updatable tipoPersona model
            var personTypeToPatch = _mapper.Map<PersonTypeForUpdatedDto>(existingPersonType);
            // apply patchdoc updates to the updatable tipoPersona
            pathDoc.ApplyTo(personTypeToPatch, ModelState);

            if (!TryValidateModel(personTypeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable tipoPersona to the db entity so we can apply the updates to the database
            _mapper.Map(personTypeToPatch, existingPersonType);
            // apply business updates to data if needed
            _personTypeRepository.UpdatePersonType(existingPersonType);

            // save changes in the database
            await _personTypeRepository.SaveAsync();

            return NoContent();
        }
    }
}
