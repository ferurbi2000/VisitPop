using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Empleado;
using VisitPop.Application.Interfaces.Empleado;
using VisitPop.Application.Validation.Empleado;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Empleados")]
    [ApiVersion("1.0")]
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoRepository _empleadoRepo;
        private readonly IMapper _mapper;

        public EmpleadosController(IEmpleadoRepository empleadoRepo,
            IMapper mapper)
        {
            _empleadoRepo = empleadoRepo ??
                throw new ArgumentNullException(nameof(empleadoRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetEmpleados")]
        public async Task<IActionResult> GetEmpleados([FromQuery] EmpleadoParametersDto empleadoParameterDto)
        {
            var empleadosFromRepo = await _empleadoRepo.GetEmpleadosAsync(empleadoParameterDto);

            var paginationMetadata = new
            {
                totalCount = empleadosFromRepo.MetaData.TotalCount,
                pageSize = empleadosFromRepo.MetaData.PageSize,
                currentPageSize = empleadosFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = empleadosFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = empleadosFromRepo.MetaData.CurrentEndIndex,
                pageNumer = empleadosFromRepo.MetaData.PageNumber,
                totalPages = empleadosFromRepo.MetaData.TotalPages,
                hasPrevious = empleadosFromRepo.MetaData.HasPrevious,
                hasNext = empleadosFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var empleadosDto = _mapper.Map<IEnumerable<EmpleadoDto>>(empleadosFromRepo);
            var response = new Response<IEnumerable<EmpleadoDto>>(empleadosDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmpleadoDto>> GetEmpleado(int Id)
        {
            var empleadoFromRepo = await _empleadoRepo.GetEmpleadoAsync(Id);

            if (empleadoFromRepo == null)
            {
                return NotFound();
            }

            var empleadoDto = _mapper.Map<EmpleadoDto>(empleadoFromRepo);
            var response = new Response<EmpleadoDto>(empleadoDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmpleadoDto>> AddEmpleado([FromBody] EmpleadoForCreationDto empleadoForCreation)
        {
            var validationResults = new EmpleadoForCreationDtoValidator().Validate(empleadoForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var empleado = _mapper.Map<Empleado>(empleadoForCreation);
            await _empleadoRepo.AddEmpleado(empleado);
            var saveSuccessful = await _empleadoRepo.SaveAsync();

            if (saveSuccessful)
            {
                var empleadoFromRepo = await _empleadoRepo.GetEmpleadoAsync(empleado.Id);
                var empleadoDto = _mapper.Map<EmpleadoDto>(empleadoFromRepo);
                var response = new Response<EmpleadoDto>(empleadoDto);

                return CreatedAtRoute("GetEmpleado",
                    new { empleadoDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEmpleado(int Id)
        {
            var empleadoFromRepo = await _empleadoRepo.GetEmpleadoAsync(Id);

            if (empleadoFromRepo == null)
            {
                return NotFound();
            }

            _empleadoRepo.DeleteEmpleado(empleadoFromRepo);
            await _empleadoRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateEmpleado(int Id, EmpleadoForUpdateDto empleado)
        {
            var empleadoFromRepo = await _empleadoRepo.GetEmpleadoAsync(Id);

            if (empleadoFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new EmpleadoForUpdateDtoValidator().Validate(empleado);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(empleado, empleadoFromRepo);
            _empleadoRepo.UpdateEmpleado(empleadoFromRepo);

            await _empleadoRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateEmpleado(int Id, JsonPatchDocument<EmpleadoForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingEmpleado = await _empleadoRepo.GetEmpleadoAsync(Id);

            if (existingEmpleado == null)
            {
                return NotFound();
            }

            var empleadoToPatch = _mapper.Map<EmpleadoForUpdateDto>(existingEmpleado); // map the empleado we got from the database to an updatable empleado model
            patchDoc.ApplyTo(empleadoToPatch, ModelState); // apply patchdoc updates to the updatable empleado

            if (!TryValidateModel(empleadoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(empleadoToPatch, existingEmpleado); // apply updates from the updatable empleado to the db entity so we can apply the updates to the database
            _empleadoRepo.UpdateEmpleado(existingEmpleado); // apply business updates to data if needed

            await _empleadoRepo.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
