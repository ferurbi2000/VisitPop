using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Observacion;
using VisitPop.Application.Interfaces.Observacion;
using VisitPop.Application.Validation.Observacion;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Observaciones")]
    [ApiVersion("1.0")]
    public class ObservacionesController : Controller
    {
        private readonly IObservacionRepository _observacionRepository;
        private readonly IMapper _mapper;

        public ObservacionesController(IObservacionRepository observacionRepository,
            IMapper mapper)
        {
            _observacionRepository = observacionRepository
                ?? throw new ArgumentNullException(nameof(observacionRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetObservaciones")]
        public async Task<IActionResult> GetObservaciones([FromQuery] ObservacionParametersDto observacionParameters)
        {
            var observacionsFromRepo = await _observacionRepository.GetObservacionesAsync(observacionParameters);

            var paginationMetadata = new
            {
                totalCount = observacionsFromRepo.MetaData.TotalCount,
                pageSize = observacionsFromRepo.MetaData.PageSize,
                currentPageSize = observacionsFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = observacionsFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = observacionsFromRepo.MetaData.CurrentEndIndex,
                pageNumber = observacionsFromRepo.MetaData.PageNumber,
                totalPages = observacionsFromRepo.MetaData.TotalPages,
                hasPrevious = observacionsFromRepo.MetaData.HasPrevious,
                hasNext = observacionsFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var observacionsDto = _mapper.Map<IEnumerable<ObservacionDto>>(observacionsFromRepo);
            var response = new Response<IEnumerable<ObservacionDto>>(observacionsDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetObservacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ObservacionDto>> GetObservacion(int id)
        {
            var observacionFromRepo = await _observacionRepository.GetObservacionAsync(id);

            if (observacionFromRepo == null)
            {
                return NotFound();
            }

            var observacionDto = _mapper.Map<ObservacionDto>(observacionFromRepo);
            var response = new Response<ObservacionDto>(observacionDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ObservacionDto>> AddObservacion([FromBody] ObservacionForCreationDto observacionForCreation)
        {
            var validationResults = new ObservacionForCreationDtoValidator().Validate(observacionForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var observacion = _mapper.Map<Observacion>(observacionForCreation);
            await _observacionRepository.AddObservacion(observacion);
            var saveSuccessful = await _observacionRepository.SaveAsync();

            if (saveSuccessful)
            {
                var observacionFromRepo = await _observacionRepository.GetObservacionAsync(observacion.Id);
                var observacionDto = _mapper.Map<ObservacionDto>(observacionFromRepo);
                var response = new Response<ObservacionDto>(observacionDto);

                return CreatedAtRoute("GetObservacion",
                    new { observacionDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteObservacion(int id)
        {
            var observacionFromRepo = await _observacionRepository.GetObservacionAsync(id);

            if (observacionFromRepo == null)
            {
                return NotFound();
            }

            _observacionRepository.DeleteObservacion(observacionFromRepo);
            await _observacionRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateObservacion(int id, ObservacionForUpdateDto observacion)
        {
            var observacionFromRepo = await _observacionRepository.GetObservacionAsync(id);

            if (observacionFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new ObservacionForUpdateDtoValidator().Validate(observacion);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(observacion, observacionFromRepo);
            _observacionRepository.UpdateObservacion(observacionFromRepo);

            await _observacionRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateObservacion(int id, JsonPatchDocument<ObservacionForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingObservacion = await _observacionRepository.GetObservacionAsync(id);

            if (existingObservacion == null)
            {
                return NotFound();
            }

            // map the observacion we got from the database to an updatable observacion model
            var observacionToPatch = _mapper.Map<ObservacionForUpdateDto>(existingObservacion);
            // apply patchdoc updates to the updatable observacion
            patchDoc.ApplyTo(observacionToPatch, ModelState);

            if (!TryValidateModel(observacionToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable observacion to the db entity so we can apply the updates to the database
            _mapper.Map(observacionToPatch, existingObservacion);
            // apply business updates to data if needed
            _observacionRepository.UpdateObservacion(existingObservacion);

            // save changes in the database
            await _observacionRepository.SaveAsync();

            return NoContent();

        }
    }
}
