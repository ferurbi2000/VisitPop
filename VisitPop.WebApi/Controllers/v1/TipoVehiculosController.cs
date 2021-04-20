using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoVehiculo;
using VisitPop.Application.Interfaces.TipoVehiculo;
using VisitPop.Application.Validation.TipoVehiculo;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/TipoVehiculos")]
    public class TipoVehiculosController : Controller
    {
        private readonly ITipoVehiculoRepository _tipoVehiculoRepo;
        private readonly IMapper _mapper;

        public TipoVehiculosController(ITipoVehiculoRepository repo,
            IMapper map)
        {
            _tipoVehiculoRepo = repo
                ?? throw new ArgumentNullException(nameof(repo));
            _mapper = map
                ?? throw new ArgumentNullException(nameof(map));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTipoVehiculos")]
        public async Task<IActionResult> GetTipoVehiculosAsync([FromQuery] TipoVehiculoParametersDto tipoVehiculoParameters)
        {
            var tipoVehiculosFromRepo = await _tipoVehiculoRepo.GetTipoVehiculosAsync(tipoVehiculoParameters);

            var paginationMetadata = new
            {
                totalCount = tipoVehiculosFromRepo.MetaData.TotalCount,
                pageSize = tipoVehiculosFromRepo.MetaData.PageSize,
                currentPageSize = tipoVehiculosFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = tipoVehiculosFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = tipoVehiculosFromRepo.MetaData.CurrentEndIndex,
                pageNumber = tipoVehiculosFromRepo.MetaData.PageNumber,
                totalPages = tipoVehiculosFromRepo.MetaData.TotalPages,
                hasPrevious = tipoVehiculosFromRepo.MetaData.HasPrevious,
                hasNext = tipoVehiculosFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tipoVehiculosDto = _mapper.Map<IEnumerable<TipoVehiculoDto>>(tipoVehiculosFromRepo);
            var response = new Response<IEnumerable<TipoVehiculoDto>>(tipoVehiculosDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetTipoVehiculo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TipoVehiculoDto>> GetTipoVehiculo(int id)
        {
            var tipoVehiculoFromRepo = await _tipoVehiculoRepo.GetTipoVehiculoAsync(id);

            if (tipoVehiculoFromRepo == null)
                return NotFound();

            var tipoVehiculoDto = _mapper.Map<TipoVehiculoDto>(tipoVehiculoFromRepo);
            var response = new Response<TipoVehiculoDto>(tipoVehiculoDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TipoVehiculoDto>> AddTipoVehiculo([FromBody] TipoVehiculoForCreationDto tipoVehiculoForCreation)
        {
            var validationResults = new TipoVehiculoForCreationDtoValidator().Validate(tipoVehiculoForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var tipoVehiculo = _mapper.Map<TipoVehiculo>(tipoVehiculoForCreation);
            await _tipoVehiculoRepo.AddTipoVehiculo(tipoVehiculo);
            var saveSucessful = await _tipoVehiculoRepo.SaveAsync();

            if (saveSucessful)
            {
                var tipoVehiculoFromRepo = await _tipoVehiculoRepo.GetTipoVehiculoAsync(tipoVehiculo.Id);
                var tipoVehiculoDto = _mapper.Map<TipoVehiculoDto>(tipoVehiculoFromRepo);
                var response = new Response<TipoVehiculoDto>(tipoVehiculoDto);

                return CreatedAtRoute("GetTipoVehiculo",
                    new { tipoVehiculo.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteTipoVehiculo(int id)
        {
            var tipoVehiculoFromRepo = await _tipoVehiculoRepo.GetTipoVehiculoAsync(id);

            if (tipoVehiculoFromRepo == null)
                return NotFound();

            _tipoVehiculoRepo.DeleteTipoVehiculo(tipoVehiculoFromRepo);
            await _tipoVehiculoRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTipoVehiculo(int id, TipoVehiculoForUpdateDto tipoVehiculo)
        {
            var tipoVehiculoFromRepo = await _tipoVehiculoRepo.GetTipoVehiculoAsync(id);

            if (tipoVehiculoFromRepo == null)
                return NotFound();

            var validationResults = new TipoVehiculoForUpdateDtoValidator().Validate(tipoVehiculo);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(tipoVehiculo, tipoVehiculoFromRepo);
            _tipoVehiculoRepo.UpdateTipoVehiculo(tipoVehiculoFromRepo);

            await _tipoVehiculoRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("applicarion/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateTipoVehiculo(int id, JsonPatchDocument<TipoVehiculoForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var existingTipoVehiculo = await _tipoVehiculoRepo.GetTipoVehiculoAsync(id);

            if (existingTipoVehiculo == null)
                return NotFound();

            // map the tipoVehiculo we got from the database to an updatable tipoVehiculo model
            var tipoVehiculoToPatch = _mapper.Map<TipoVehiculoForUpdateDto>(existingTipoVehiculo);
            // apply patchdoc updates to the updatable tipoVehiculo
            patchDoc.ApplyTo(tipoVehiculoToPatch, ModelState);

            if (!TryValidateModel(tipoVehiculoToPatch))
                return ValidationProblem(ModelState);

            // apply updates from the updatable tipoVehiculo to the db entity so we can apply the updates to the database
            _mapper.Map(tipoVehiculoToPatch, existingTipoVehiculo);
            // apply business updates to data if needed
            _tipoVehiculoRepo.UpdateTipoVehiculo(existingTipoVehiculo);

            // save changes in the database
            await _tipoVehiculoRepo.SaveAsync();

            return NoContent();
        }
    }
}
