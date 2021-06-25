using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Interfaces.Office;
using VisitPop.Application.Validation.Office;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/Offices")]
    [ApiVersion("1.0")]
    public class OfficesController : Controller
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public OfficesController(IOfficeRepository officeRepository,
            IMapper mapper)
        {
            _officeRepository = officeRepository
                ?? throw new ArgumentNullException(nameof(officeRepository));

            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetOffices")]
        public async Task<IActionResult> GetOffices([FromQuery] OfficeParametersDto officeParametersDto)
        {
            var officesFromRepo = await _officeRepository.GetOfficesAsync(officeParametersDto);

            var paginationMetadata = new
            {
                totalCount = officesFromRepo.MetaData.TotalCount,
                pageSize = officesFromRepo.MetaData.PageSize,
                currentPageSize = officesFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = officesFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = officesFromRepo.MetaData.CurrentEndIndex,
                pageNumber = officesFromRepo.MetaData.PageNumber,
                totalPages = officesFromRepo.MetaData.TotalPages,
                hasPrevious = officesFromRepo.MetaData.HasPrevious,
                hasNext = officesFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var officesDto = _mapper.Map<IEnumerable<OfficeDto>>(officesFromRepo);
            var response = new Response<IEnumerable<OfficeDto>>(officesDto);

            return Ok(response);

        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetOffice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OfficeDto>> GetOffice(int id)
        {
            var officeFromRepo = await _officeRepository.GetOfficeAsync(id);

            if (officeFromRepo == null)
            {
                return NotFound();
            }

            var officeDto = _mapper.Map<OfficeDto>(officeFromRepo);
            var response = new Response<OfficeDto>(officeDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OfficeDto>> AddOffice([FromBody] OfficeForCreationDto officeForCreation)
        {
            var validationResults = new OfficeForCreationDtoValidator().Validate(officeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var office = _mapper.Map<Office>(officeForCreation);
            await _officeRepository.AddOffice(office);
            var saveSuccessful = await _officeRepository.SaveAsync();

            if (saveSuccessful)
            {
                var officeFromRepo = await _officeRepository.GetOfficeAsync(office.Id);
                var officeDto = _mapper.Map<OfficeDto>(officeFromRepo);
                var response = new Response<OfficeDto>(officeDto);

                return CreatedAtRoute("GetOffice",
                    new { officeDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOffice(int id)
        {
            var officeFromRepo = await _officeRepository.GetOfficeAsync(id);

            if (officeFromRepo == null)
            {
                return NotFound();
            }

            _officeRepository.DeleteOffice(officeFromRepo);
            await _officeRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOffice(int id, OfficeForUpdateDto office)
        {
            var officeFromRepo = await _officeRepository.GetOfficeAsync(id);

            if (officeFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new OfficeForUpdateDtoValidator().Validate(office);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(office, officeFromRepo);
            _officeRepository.UpdateOffice(officeFromRepo);

            await _officeRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateOffice(int id, JsonPatchDocument<OfficeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingOffice = await _officeRepository.GetOfficeAsync(id);

            if (existingOffice == null)
            {
                return NotFound();
            }

            // map the oficina we got from the database to an updatable oficina model
            var officeToPatch = _mapper.Map<OfficeForUpdateDto>(existingOffice);
            // apply patchdoc updates to the updatable oficina
            patchDoc.ApplyTo(officeToPatch, ModelState);

            if (!TryValidateModel(officeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // apply updates from the updatable oficina to the db entity so we can apply the updates to the database
            _mapper.Map(officeToPatch, existingOffice);
            // apply business updates to data if needed
            _officeRepository.UpdateOffice(existingOffice);

            // save changes in the database
            await _officeRepository.SaveAsync();

            return NoContent();
        }
    }
}
