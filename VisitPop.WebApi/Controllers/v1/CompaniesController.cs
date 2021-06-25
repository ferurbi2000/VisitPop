using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Interfaces.Company;
using VisitPop.Application.Validation.Company;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Companies")]
    [ApiVersion("1.0")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepo,
            IMapper mapper)
        {
            _companyRepo = companyRepo
                ?? throw new ArgumentNullException(nameof(companyRepo));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetCompanies")]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyParametersDto companyParametersDto)
        {
            var companiesFromRepo = await _companyRepo.GetCompaniesAsync(companyParametersDto);

            var paginationMetadata = new
            {
                totalCount = companiesFromRepo.MetaData.TotalCount,
                pageSize = companiesFromRepo.MetaData.PageSize,
                currentPageSize = companiesFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = companiesFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = companiesFromRepo.MetaData.CurrentEndIndex,
                pageNumber = companiesFromRepo.MetaData.PageNumber,
                totalPages = companiesFromRepo.MetaData.TotalPages,
                hasPrevious = companiesFromRepo.MetaData.HasPrevious,
                hasNext = companiesFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesFromRepo);
            var response = new Response<IEnumerable<CompanyDto>>(companiesDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{Id}", Name = "GetCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CompanyDto>> GetCompany(int Id)
        {
            var companyFromRepo = await _companyRepo.GetCompanyAsync(Id);
            if (companyFromRepo == null)
            {
                return NotFound();
            }

            var companyDto = _mapper.Map<CompanyDto>(companyFromRepo);
            var response = new Response<CompanyDto>(companyDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CompanyDto>> AddCompany([FromBody] CompanyForCreationDto companyForCreation)
        {
            var validationResults = new CompanyForCreationDtoValidator().Validate(companyForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var company = _mapper.Map<Company>(companyForCreation);
            await _companyRepo.AddCompany(company);
            var saveSuccessful = await _companyRepo.SaveAsync();

            if (saveSuccessful)
            {
                var companyFromRepo = await _companyRepo.GetCompanyAsync(company.Id);
                var companyDto = _mapper.Map<CompanyDto>(companyFromRepo);
                var response = new Response<CompanyDto>(companyDto);

                return CreatedAtRoute("GetCompany",
                    new { company.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCompany(int Id)
        {
            var companyFromRepo = await _companyRepo.GetCompanyAsync(Id);

            if (companyFromRepo == null)
            {
                return NotFound();
            }

            _companyRepo.DeleteCompany(companyFromRepo);
            await _companyRepo.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCompany(int Id, CompanyForUpdateDto company)
        {
            var companyFromRepo = await _companyRepo.GetCompanyAsync(Id);

            if (companyFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new CompanyForUpdateDtoValidator().Validate(company);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(company, companyFromRepo);
            _companyRepo.UpdateCompany(companyFromRepo);

            await _companyRepo.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateCompany(int Id, JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingCompany = await _companyRepo.GetCompanyAsync(Id);

            if (existingCompany == null)
            {
                return NotFound();
            }

            var companyToPatch = _mapper.Map<CompanyForUpdateDto>(existingCompany); // map the company we got from the database to an updatable empresa model
            patchDoc.ApplyTo(companyToPatch, ModelState); // apply patchdoc updates to the updatable company

            if (!TryValidateModel(companyToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(companyToPatch, existingCompany); // apply updates from the updatable company to the db entity so we can apply the updates to the database
            _companyRepo.UpdateCompany(existingCompany); // apply business updates to data if needed

            await _companyRepo.SaveAsync(); // save changes in the database

            return NoContent();
        }

    }
}
