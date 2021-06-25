using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.Application.Interfaces.VehicleType;
using VisitPop.Application.Validation.VehicleType;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/VehicleTypes")]
    public class VehicleTypesController : Controller
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepo;
        private readonly IMapper _mapper;

        public VehicleTypesController(IVehicleTypeRepository repo,
            IMapper map)
        {
            _vehicleTypeRepo = repo
                ?? throw new ArgumentNullException(nameof(repo));
            _mapper = map
                ?? throw new ArgumentNullException(nameof(map));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVehicleTypes")]
        public async Task<IActionResult> GetVehicleTypesAsync([FromQuery] VehicleTypeParametersDto vehicleTypeParameters)
        {
            var vehicleTypesFromRepo = await _vehicleTypeRepo.GetVehicleTypesAsync(vehicleTypeParameters);

            var paginationMetadata = new
            {
                totalCount = vehicleTypesFromRepo.MetaData.TotalCount,
                pageSize = vehicleTypesFromRepo.MetaData.PageSize,
                currentPageSize = vehicleTypesFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = vehicleTypesFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = vehicleTypesFromRepo.MetaData.CurrentEndIndex,
                pageNumber = vehicleTypesFromRepo.MetaData.PageNumber,
                totalPages = vehicleTypesFromRepo.MetaData.TotalPages,
                hasPrevious = vehicleTypesFromRepo.MetaData.HasPrevious,
                hasNext = vehicleTypesFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var vehicleTypesDto = _mapper.Map<IEnumerable<VehicleTypeDto>>(vehicleTypesFromRepo);
            var response = new Response<IEnumerable<VehicleTypeDto>>(vehicleTypesDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetVehicleType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VehicleTypeDto>> GetVehicleType(int id)
        {
            var vehicleTypeFromRepo = await _vehicleTypeRepo.GetVehicleTypeAsync(id);

            if (vehicleTypeFromRepo == null)
                return NotFound();

            var vehicleTypeDto = _mapper.Map<VehicleTypeDto>(vehicleTypeFromRepo);
            var response = new Response<VehicleTypeDto>(vehicleTypeDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VehicleTypeDto>> AddVehicleType([FromBody] VehicleTypeForCreationDto vehicleTypeForCreation)
        {
            var validationResults = new VehicleTypeForCreationDtoValidator().Validate(vehicleTypeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var vehicleType = _mapper.Map<VehicleType>(vehicleTypeForCreation);
            await _vehicleTypeRepo.AddVehicleType(vehicleType);
            var saveSucessful = await _vehicleTypeRepo.SaveAsync();

            if (saveSucessful)
            {
                var vehicleTypeFromRepo = await _vehicleTypeRepo.GetVehicleTypeAsync(vehicleType.Id);
                var vehicleTypeDto = _mapper.Map<VehicleTypeDto>(vehicleTypeFromRepo);
                var response = new Response<VehicleTypeDto>(vehicleTypeDto);

                return CreatedAtRoute("GetVehicleType",
                    new { vehicleType.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVehicleType(int id)
        {
            var vehicleTypeFromRepo = await _vehicleTypeRepo.GetVehicleTypeAsync(id);

            if (vehicleTypeFromRepo == null)
                return NotFound();

            _vehicleTypeRepo.DeleteVehicleType(vehicleTypeFromRepo);
            await _vehicleTypeRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVehicleType(int id, VehicleTypeForUpdateDto vehicleType)
        {
            var vehicleTypeFromRepo = await _vehicleTypeRepo.GetVehicleTypeAsync(id);

            if (vehicleTypeFromRepo == null)
                return NotFound();

            var validationResults = new VehicleTypeForUpdateDtoValidator().Validate(vehicleType);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(vehicleType, vehicleTypeFromRepo);
            _vehicleTypeRepo.UpdateVehicleType(vehicleTypeFromRepo);

            await _vehicleTypeRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("applicarion/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVehicleType(int id, JsonPatchDocument<VehicleTypeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var existingVehicleType = await _vehicleTypeRepo.GetVehicleTypeAsync(id);

            if (existingVehicleType == null)
                return NotFound();

            // map the tipoVehiculo we got from the database to an updatable tipoVehiculo model
            var vehicleTypeToPatch = _mapper.Map<VehicleTypeForUpdateDto>(existingVehicleType);
            // apply patchdoc updates to the updatable tipoVehiculo
            patchDoc.ApplyTo(vehicleTypeToPatch, ModelState);

            if (!TryValidateModel(vehicleTypeToPatch))
                return ValidationProblem(ModelState);

            // apply updates from the updatable tipoVehiculo to the db entity so we can apply the updates to the database
            _mapper.Map(vehicleTypeToPatch, existingVehicleType);
            // apply business updates to data if needed
            _vehicleTypeRepo.UpdateVehicleType(existingVehicleType);

            // save changes in the database
            await _vehicleTypeRepo.SaveAsync();

            return NoContent();
        }
    }
}
