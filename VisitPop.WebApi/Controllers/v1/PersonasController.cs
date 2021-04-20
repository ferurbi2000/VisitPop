using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Persona;
using VisitPop.Application.Interfaces.Persona;
using VisitPop.Application.Validation.Persona;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Personas")]
    [ApiVersion("1.0")]
    public class PersonasController : Controller
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;

        public PersonasController(IPersonaRepository personaRepository,
            IMapper mapper)
        {
            _personaRepository = personaRepository
                ?? throw new ArgumentNullException(nameof(personaRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetPersonas")]
        public async Task<IActionResult> GetPersonasAsync([FromQuery] PersonaParametersDto personaParameters)
        {
            var personasFromRepo = await _personaRepository.GetPersonasAsync(personaParameters);

            var paginationMetadata = new
            {
                totalCount = personasFromRepo.MetaData.TotalCount,
                pageSize = personasFromRepo.MetaData.PageSize,
                currentPageSize = personasFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = personasFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = personasFromRepo.MetaData.CurrentEndIndex,
                pageNumber = personasFromRepo.MetaData.PageNumber,
                totalPages = personasFromRepo.MetaData.TotalPages,
                hasPrevious = personasFromRepo.MetaData.HasPrevious,
                hasNext = personasFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var personasDto = _mapper.Map<IEnumerable<PersonaDto>>(personasFromRepo);
            var response = new Response<IEnumerable<PersonaDto>>(personasDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PersonaDto>> GetPersona(int id)
        {
            var personaFromRepo = await _personaRepository.GetPersonaAsync(id);

            if (personaFromRepo == null)
                return NotFound();

            var personaDto = _mapper.Map<PersonaDto>(personaFromRepo);
            var response = new Response<PersonaDto>(personaDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PersonaDto>> AddPersona([FromBody] PersonaForCreationDto personaForCreation)
        {
            var validationResults = new PersonaForCreationDtoValidator().Validate(personaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var persona = _mapper.Map<Persona>(personaForCreation);
            await _personaRepository.AddPersona(persona);
            var saveSucessful = await _personaRepository.SaveAsync();

            if (saveSucessful)
            {
                var personaFromRepo = await _personaRepository.GetPersonaAsync(persona.Id);
                var personaDto = _mapper.Map<PersonaDto>(personaFromRepo);
                var response = new Response<PersonaDto>(personaDto);

                return CreatedAtRoute("GetPersona",
                    new { personaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeletePersona(int id)
        {
            var personaFromRepo = await _personaRepository.GetPersonaAsync(id);

            if (personaFromRepo == null)
                return NotFound();

            _personaRepository.DeletePersona(personaFromRepo);
            await _personaRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePersona(int id, PersonaForUpdateDto persona)
        {
            var personFromRepo = await _personaRepository.GetPersonaAsync(id);

            if (personFromRepo == null)
                return NotFound();

            var validationResults = new PersonaForUpdateDtoValidator().Validate(persona);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(persona, personFromRepo);
            _personaRepository.UpdatePersona(personFromRepo);

            await _personaRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdatePersona(int id, JsonPatchDocument<PersonaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingPersona = await _personaRepository.GetPersonaAsync(id);

            if (existingPersona == null)
            {
                return NotFound();
            }

            var personaToPatch = _mapper.Map<PersonaForUpdateDto>(existingPersona); // map the persona we got from the database to an updatable persona model
            patchDoc.ApplyTo(personaToPatch, ModelState); // apply patchdoc updates to the updatable persona

            if (!TryValidateModel(personaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(personaToPatch, existingPersona); // apply updates from the updatable persona to the db entity so we can apply the updates to the database
            _personaRepository.UpdatePersona(existingPersona); // apply business updates to data if needed

            await _personaRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
