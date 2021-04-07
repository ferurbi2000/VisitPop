using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Oficina;
using VisitPop.Application.Interfaces.Oficina;
using VisitPop.Application.Validation.Oficina;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Oficinas")]
    [ApiVersion("1.0")]
    public class OficinaController : Controller
    {
        private readonly IOficinaRepository _oficinaRepository;
        private readonly IMapper _mapper;

        public OficinaController(IOficinaRepository oficinaRepository,
            IMapper mapper)
        {
            _oficinaRepository = oficinaRepository
                ?? throw new ArgumentNullException(nameof(oficinaRepository));

            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetOficinas")]
        public async Task<IActionResult> GetOficinas([FromQuery] OficinaParametersDto oficinaParametersDto)
        {
            var oficinasFromRepo = await _oficinaRepository.GetOficinasAsync(oficinaParametersDto);

            var paginationMetadata = new
            {
                totalCount = oficinasFromRepo.TotalCount,
                pageSize = oficinasFromRepo.PageSize,
                currentPageSize = oficinasFromRepo.CurrentPageSize,
                currentStartIndex = oficinasFromRepo.CurrentStartIndex,
                currentEndIndex = oficinasFromRepo.CurrentEndIndex,
                pageNumber = oficinasFromRepo.PageNumber,
                totalPages = oficinasFromRepo.TotalPages,
                hasPrevious = oficinasFromRepo.HasPrevious,
                hasNext = oficinasFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var oficinasDto = _mapper.Map<IEnumerable<OficinaDto>>(oficinasFromRepo);
            var response = new Response<IEnumerable<OficinaDto>>(oficinasDto);

            return Ok(response);

        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetOficina")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OficinaDto>> GetOficina(int id)
        {
            var oficinaFromRepo = await _oficinaRepository.GetOficinaAsync(id);

            if (oficinaFromRepo == null)
            {
                return NotFound();
            }

            var oficinaDto = _mapper.Map<OficinaDto>(oficinaFromRepo);
            var response = new Response<OficinaDto>(oficinaDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OficinaDto>> AddOficina([FromBody] OficinaForCreationDto oficinaForCreation)
        {
            var validationResults = new OficinaForCreationDtoValidator().Validate(oficinaForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var oficina = _mapper.Map<Oficina>(oficinaForCreation);
            await _oficinaRepository.AddOficina(oficina);
            var saveSuccessful = await _oficinaRepository.SaveAsync();

            if (saveSuccessful)
            {
                var oficinaFromRepo = await _oficinaRepository.GetOficinaAsync(oficina.Id);
                var oficinaDto = _mapper.Map<OficinaDto>(oficinaFromRepo);
                var response = new Response<OficinaDto>(oficinaDto);

                return CreatedAtRoute("GetOficina",
                    new { oficinaDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOficina(int id)
        {
            var oficinaFromRepo = await _oficinaRepository.GetOficinaAsync(id);

            if (oficinaFromRepo == null)
            {
                return NotFound();
            }

            _oficinaRepository.DeleteOficina(oficinaFromRepo);
            await _oficinaRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOficina(int id, OficinaForUpdateDto oficina)
        {
            var oficinaFromRepo = await _oficinaRepository.GetOficinaAsync(id);

            if (oficinaFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new OficinaForUpdateDtoValidator().Validate(oficina);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(oficina, oficinaFromRepo);
            _oficinaRepository.UpdateOficina(oficinaFromRepo);

            await _oficinaRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateOficina(int id, JsonPatchDocument<OficinaForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingOficina = await _oficinaRepository.GetOficinaAsync(id);

            if (existingOficina == null)
            {
                return NotFound();
            }

            // map the oficina we got from the database to an updatable oficina model
            var oficinaToPatch = _mapper.Map<OficinaForUpdateDto>(existingOficina);
            // apply patchdoc updates to the updatable oficina
            patchDoc.ApplyTo(oficinaToPatch, ModelState);

            if (!TryValidateModel(oficinaToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable oficina to the db entity so we can apply the updates to the database
            _mapper.Map(oficinaToPatch, existingOficina);
            // apply business updates to data if needed
            _oficinaRepository.UpdateOficina(existingOficina);

            // save changes in the database
            await _oficinaRepository.SaveAsync();

            return NoContent();
        }
    }
}
