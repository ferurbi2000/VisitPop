using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Interfaces.Person;
using VisitPop.Application.Validation.Person;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Persons")]
    [ApiVersion("1.0")]
    public class PersonsController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonsController(IPersonRepository personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository
                ?? throw new ArgumentNullException(nameof(personRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetPersons")]
        public async Task<IActionResult> GetPersonsAsync([FromQuery] PersonParametersDto personParameters)
        {
            var personsFromRepo = await _personRepository.GetPersonsAsync(personParameters);

            var paginationMetadata = new
            {
                totalCount = personsFromRepo.MetaData.TotalCount,
                pageSize = personsFromRepo.MetaData.PageSize,
                currentPageSize = personsFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = personsFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = personsFromRepo.MetaData.CurrentEndIndex,
                pageNumber = personsFromRepo.MetaData.PageNumber,
                totalPages = personsFromRepo.MetaData.TotalPages,
                hasPrevious = personsFromRepo.MetaData.HasPrevious,
                hasNext = personsFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(personsFromRepo);
            var response = new Response<IEnumerable<PersonDto>>(personsDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var personFromRepo = await _personRepository.GetPersonAsync(id);

            if (personFromRepo == null)
                return NotFound();

            var personDto = _mapper.Map<PersonDto>(personFromRepo);
            var response = new Response<PersonDto>(personDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PersonDto>> AddPerson([FromBody] PersonForCreationDto personForCreation)
        {
            var validationResults = new PersonForCreationDtoValidator().Validate(personForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var person = _mapper.Map<Person>(personForCreation);
            await _personRepository.AddPerson(person);
            var saveSucessful = await _personRepository.SaveAsync();

            if (saveSucessful)
            {
                var personFromRepo = await _personRepository.GetPersonAsync(person.Id);
                var personDto = _mapper.Map<PersonDto>(personFromRepo);
                var response = new Response<PersonDto>(personDto);

                return CreatedAtRoute("GetPerson",
                    new { personDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var personFromRepo = await _personRepository.GetPersonAsync(id);

            if (personFromRepo == null)
                return NotFound();

            _personRepository.DeletePerson(personFromRepo);
            await _personRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePerson(int id, PersonForUpdateDto person)
        {
            var personFromRepo = await _personRepository.GetPersonAsync(id);

            if (personFromRepo == null)
                return NotFound();

            var validationResults = new PersonForUpdateDtoValidator().Validate(person);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(person, personFromRepo);
            _personRepository.UpdatePerson(personFromRepo);

            await _personRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdatePerson(int id, JsonPatchDocument<PersonForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingPerson = await _personRepository.GetPersonAsync(id);

            if (existingPerson == null)
            {
                return NotFound();
            }

            var personToPatch = _mapper.Map<PersonForUpdateDto>(existingPerson); // map the persona we got from the database to an updatable persona model
            patchDoc.ApplyTo(personToPatch, ModelState); // apply patchdoc updates to the updatable persona

            if (!TryValidateModel(personToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(personToPatch, existingPerson); // apply updates from the updatable persona to the db entity so we can apply the updates to the database
            _personRepository.UpdatePerson(existingPerson); // apply business updates to data if needed

            await _personRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
