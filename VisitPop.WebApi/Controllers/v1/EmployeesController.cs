using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Interfaces.Employee;
using VisitPop.Application.Validation.Employee;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Employees")]
    [ApiVersion("1.0")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepo,
            IMapper mapper)
        {
            _employeeRepo = employeeRepo ??
                throw new ArgumentNullException(nameof(employeeRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetEmployees")]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeParametersDto employeeParameterDto)
        {
            var employeesFromRepo = await _employeeRepo.GetEmployeesAsync(employeeParameterDto);

            var paginationMetadata = new
            {
                totalCount = employeesFromRepo.MetaData.TotalCount,
                pageSize = employeesFromRepo.MetaData.PageSize,
                currentPageSize = employeesFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = employeesFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = employeesFromRepo.MetaData.CurrentEndIndex,
                pageNumber = employeesFromRepo.MetaData.PageNumber,
                totalPages = employeesFromRepo.MetaData.TotalPages,
                hasPrevious = employeesFromRepo.MetaData.HasPrevious,
                hasNext = employeesFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromRepo);
            var response = new Response<IEnumerable<EmployeeDto>>(employeesDto);

            return Ok(response);
        }
        

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int Id)
        {
            var employeeFromRepo = await _employeeRepo.GetEmployeeAsync(Id);

            if (employeeFromRepo == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employeeFromRepo);
            var response = new Response<EmployeeDto>(employeeDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmployeeDto>> AddEmployee([FromBody] EmployeeForCreationDto employeeForCreation)
        {
            var validationResults = new EmployeeForCreationDtoValidator().Validate(employeeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var employee = _mapper.Map<Employee>(employeeForCreation);
            await _employeeRepo.AddEmployee(employee);
            var saveSuccessful = await _employeeRepo.SaveAsync();

            if (saveSuccessful)
            {
                var employeeFromRepo = await _employeeRepo.GetEmployeeAsync(employee.Id);
                var employeeDto = _mapper.Map<EmployeeDto>(employeeFromRepo);
                var response = new Response<EmployeeDto>(employeeDto);

                return CreatedAtRoute("GetEmployee",
                    new { employeeDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEmployee(int Id)
        {
            var employeeFromRepo = await _employeeRepo.GetEmployeeAsync(Id);

            if (employeeFromRepo == null)
            {
                return NotFound();
            }

            _employeeRepo.DeleteEmployee(employeeFromRepo);
            await _employeeRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateEmployee(int Id, EmployeeForUpdateDto employee)
        {
            var employeeFromRepo = await _employeeRepo.GetEmployeeAsync(Id);

            if (employeeFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new EmployeeForUpdateDtoValidator().Validate(employee);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(employee, employeeFromRepo);
            _employeeRepo.UpdateEmployee(employeeFromRepo);

            await _employeeRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateEmployee(int Id, JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingEmployee = await _employeeRepo.GetEmployeeAsync(Id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(existingEmployee); // map the empleado we got from the database to an updatable empleado model
            patchDoc.ApplyTo(employeeToPatch, ModelState); // apply patchdoc updates to the updatable empleado

            if (!TryValidateModel(employeeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(employeeToPatch, existingEmployee); // apply updates from the updatable empleado to the db entity so we can apply the updates to the database
            _employeeRepo.UpdateEmployee(existingEmployee); // apply business updates to data if needed

            await _employeeRepo.SaveAsync(); // save changes in the database

            return NoContent();
        }

        
    }
}
