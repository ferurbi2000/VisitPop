using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Interfaces.EmployeeDepartment;
using VisitPop.Application.Validation.EmployeeDepartment;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/EmployeeDepartments")]
    [ApiVersion("1.0")]
    public class EmployeeDepartmentsController : Controller
    {
        private readonly IEmployeeDepartmentRepository _EmployeeDepartmentRepository;
        private readonly IMapper _mapper;

        public EmployeeDepartmentsController(IEmployeeDepartmentRepository employeeDepartmentRepository
            , IMapper mapper)
        {
            _EmployeeDepartmentRepository = employeeDepartmentRepository ??
                throw new ArgumentNullException(nameof(employeeDepartmentRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetEmployeeDepartments")]
        public async Task<IActionResult> GetEmployeeDepartments([FromQuery] EmployeeDepartmentParametersDto employeeDepartmentParametersDto)
        {
            var EmployeeDepartmentsFromRepo = await _EmployeeDepartmentRepository.GetEmployeeDepartmentsAsync(employeeDepartmentParametersDto);

            var paginationMetadata = new
            {
                totalCount = EmployeeDepartmentsFromRepo.MetaData.TotalCount,
                pageSize = EmployeeDepartmentsFromRepo.MetaData.PageSize,
                currentPageSize = EmployeeDepartmentsFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = EmployeeDepartmentsFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = EmployeeDepartmentsFromRepo.MetaData.CurrentEndIndex,
                pageNumber = EmployeeDepartmentsFromRepo.MetaData.PageNumber,
                totalPages = EmployeeDepartmentsFromRepo.MetaData.TotalPages,
                hasPrevious = EmployeeDepartmentsFromRepo.MetaData.HasPrevious,
                hasNext = EmployeeDepartmentsFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var EmployeeDepartmentsDto = _mapper.Map<IEnumerable<EmployeeDepartmentDto>>(EmployeeDepartmentsFromRepo);
            var response = new Response<IEnumerable<EmployeeDepartmentDto>>(EmployeeDepartmentsDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetEmployeeDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmployeeDepartmentDto>> GetEmployeeDepartment(int Id)
        {
            var EmployeeDepartmentFromRepo = await _EmployeeDepartmentRepository.GetEmployeeDepartmentAsync(Id);

            if (EmployeeDepartmentFromRepo == null)
            {
                return NotFound();
            }

            var EmployeeDepartmentDto = _mapper.Map<EmployeeDepartmentDto>(EmployeeDepartmentFromRepo);
            var response = new Response<EmployeeDepartmentDto>(EmployeeDepartmentDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<EmployeeDepartmentDto>> AddEmployeeDepartment([FromBody] EmployeeDepartmentForCreationDto employeeDepartmentForCreation)
        {
            var validationResults = new EmployeeDepartmentForCreationDtoValidator().Validate(employeeDepartmentForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var EmployeeDepartment = _mapper.Map<EmployeeDepartment>(employeeDepartmentForCreation);
            await _EmployeeDepartmentRepository.AddEmployeeDepartment(EmployeeDepartment);
            var saveSuccessful = await _EmployeeDepartmentRepository.SaveAsync();

            if (saveSuccessful)
            {
                var EmployeeDepartmentFromRepo = await _EmployeeDepartmentRepository.GetEmployeeDepartmentAsync(EmployeeDepartment.Id);
                var EmployeeDepartmentDto = _mapper.Map<EmployeeDepartmentDto>(EmployeeDepartmentFromRepo);
                var response = new Response<EmployeeDepartmentDto>(EmployeeDepartmentDto);

                return CreatedAtRoute("GetEmployeeDepartment",
                    new { EmployeeDepartment.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteEmployeeDepartment(int Id)
        {
            var EmployeeDepartmentFromRepo = await _EmployeeDepartmentRepository.GetEmployeeDepartmentAsync(Id);

            if (EmployeeDepartmentFromRepo == null)
            {
                return NotFound();
            }

            _EmployeeDepartmentRepository.DeleteEmployeeDepartment(EmployeeDepartmentFromRepo);
            await _EmployeeDepartmentRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateEmployeeDepartment(int Id, EmployeeDepartmentForUpdateDto employeeDepartment)
        {
            var EmployeeDepartmentFromRepo = await _EmployeeDepartmentRepository.GetEmployeeDepartmentAsync(Id);

            if (EmployeeDepartmentFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new EmployeeDepartmentForUpdateDtoValidator().Validate(employeeDepartment);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(employeeDepartment, EmployeeDepartmentFromRepo);
            _EmployeeDepartmentRepository.UpdateEmployeeDepartment(EmployeeDepartmentFromRepo);

            await _EmployeeDepartmentRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateEmployeeDepartment(int Id, JsonPatchDocument<EmployeeDepartmentForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingEmployeeDepartment = await _EmployeeDepartmentRepository.GetEmployeeDepartmentAsync(Id);

            if (existingEmployeeDepartment == null)
            {
                return NotFound();
            }

            // map the DepartamentoEmpleado we got from the database to an updatable DepartamentoEmpleado model
            var EmployeeDepartmentToPatch = _mapper.Map<EmployeeDepartmentForUpdateDto>(existingEmployeeDepartment);
            // apply patchdoc updates to the updatable DepartamentoEmpleado
            patchDoc.ApplyTo(EmployeeDepartmentToPatch, ModelState);


            if (!TryValidateModel(EmployeeDepartmentToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable DepartamentoEmpleado to the db entity so we can apply the updates to the database
            _mapper.Map(EmployeeDepartmentToPatch, existingEmployeeDepartment);
            // apply business updates to data if needed
            _EmployeeDepartmentRepository.UpdateEmployeeDepartment(existingEmployeeDepartment);

            // save changes in the database
            await _EmployeeDepartmentRepository.SaveAsync();

            return NoContent();
        }
    }
}
