using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Interfaces.VisitPop;
using VisitPop.Application.Validation.Person;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/People")]
    [ApiVersion("1.0")]
    public class PeopleController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PeopleController(IPersonRepository personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name ="GetPeople")]
        public async Task<IActionResult> GetPeople([FromQuery] PersonParametersDto personParametersDto)
        {
            var peopleFromRepo = await _personRepository.GetPersonsAsync(personParametersDto);

            var paginationMetadata = new
            {
                totalCount = peopleFromRepo.TotalCount,
                pageSize = peopleFromRepo.PageSize,
                currentPageSize = peopleFromRepo.CurrentPageSize,
                currentStartIndex = peopleFromRepo.CurrentStartIndex,
                currentEndIndex = peopleFromRepo.CurrentEndIndex,
                pageNumber = peopleFromRepo.PageNumber,
                totalPages = peopleFromRepo.TotalPages,
                hasPrevious = peopleFromRepo.HasPrevious,
                hasNext = peopleFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var peopleDto = _mapper.Map<IEnumerable<PersonDto>>(peopleFromRepo);
            var response = new Response<IEnumerable<PersonDto>>(peopleDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name ="GetPerson")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var personFromRepo = await _personRepository.GetPersonAsync(id);

            if (personFromRepo == null)
            {
                return NotFound();
            }

            var personDto = _mapper.Map<PersonDto>(personFromRepo);
            var response = new Response<PersonDto>(personDto);

            return Ok(response);
        }
        
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<PersonDto>> AddPerson([FromBody] PersonForCreationDto personForCreationDto)
        {
            var validationResults = new PersonForCreationDtoValidator().Validate(personForCreationDto);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var person = _mapper.Map<Person>(personForCreationDto);
            await _personRepository.AddPerson(person);
            var saveSuccessful = await _personRepository.SaveAsync();

            if (saveSuccessful)
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
        public async Task<ActionResult> DeletePerson(int id)
        {
            var personFromRepo = await _personRepository.GetPersonAsync(id);

            if (personFromRepo == null)
            {
                return NotFound();
            }

            _personRepository.DeletePerson(personFromRepo);
            await _personRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonForUpdateDto person)
        {
            var personFromRepo = await _personRepository.GetPersonAsync(id);

            if(personFromRepo == null)
            {
                return NotFound();
            }

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

            var personToPatch = _mapper.Map<PersonForUpdateDto>(existingPerson); // map the person we got from the database to an updatable person model
            patchDoc.ApplyTo(personToPatch, ModelState); // apply patchdoc updates to the updatable person

            if (!TryValidateModel(personToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(personToPatch, existingPerson); // apply updates from the updatable person to the db entity so we can apply the updates to the database
            _personRepository.UpdatePerson(existingPerson); // apply business updates to data if needed

            await _personRepository.SaveAsync(); // save changes n the database

            return NoContent();
        }
    }    
}
