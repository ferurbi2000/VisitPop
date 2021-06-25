using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.Application.Interfaces.VisitType;
using VisitPop.Application.Validation.VisitType;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController()]
    [Route("api/VisitTypes")]
    [ApiVersion("1.0")]
    public class VisitTypesController : Controller
    {
        private readonly IVisitTypeRepository _visitTypeRepository;
        private readonly IMapper _mapper;

        public VisitTypesController(IVisitTypeRepository visitTypeRepository
           , IMapper mapper)
        {
            _visitTypeRepository = visitTypeRepository ??
                throw new ArgumentNullException(nameof(visitTypeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetVisitTypes")]
        public async Task<IActionResult> GetVisitTypes([FromQuery] VisitTypeParametersDto visitTypeParametersDto)
        {
            var visitTypessFromRepo = await _visitTypeRepository.GetVisitTypesAsync(visitTypeParametersDto);

            var paginationMetadata = new
            {
                totalCount = visitTypessFromRepo.MetaData.TotalCount,
                pageSize = visitTypessFromRepo.MetaData.PageSize,
                currentPageSize = visitTypessFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = visitTypessFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = visitTypessFromRepo.MetaData.CurrentEndIndex,
                pageNumber = visitTypessFromRepo.MetaData.PageNumber,
                totalPages = visitTypessFromRepo.MetaData.TotalPages,
                hasPrevious = visitTypessFromRepo.MetaData.HasPrevious,
                hasNext = visitTypessFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var visitTypessDto = _mapper.Map<IEnumerable<VisitTypeDto>>(visitTypessFromRepo);
            var response = new Response<IEnumerable<VisitTypeDto>>(visitTypessDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetVisitType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitTypeDto>> GetVisitType(int id)
        {
            var visitTypeFromRepo = await _visitTypeRepository.GetVisitTypeAsync(id);

            if (visitTypeFromRepo == null)
            {
                return NotFound();
            }

            var visitTypeDto = _mapper.Map<VisitTypeDto>(visitTypeFromRepo);
            var response = new Response<VisitTypeDto>(visitTypeDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<VisitTypeDto>> AddVisitType([FromBody] VisitTypeForCreationDto visitTypeForCreation)
        {
            var validationResults = new VisitTypeForCreationDtoValidator().Validate(visitTypeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var visitType = _mapper.Map<VisitType>(visitTypeForCreation);
            await _visitTypeRepository.AddVisitType(visitType);
            var saveSuccessful = await _visitTypeRepository.SaveAsync();

            if (saveSuccessful)
            {
                VisitType visitTypeFromRepo = await _visitTypeRepository.GetVisitTypeAsync(visitType.Id);
                var visitTypeDto = _mapper.Map<VisitTypeDto>(visitTypeFromRepo);
                var response = new Response<VisitTypeDto>(visitTypeDto);

                return CreatedAtRoute("GetVisitType",
                    new { visitTypeDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVisiType(int id)
        {
            var visitTypeFromRepo = await _visitTypeRepository.GetVisitTypeAsync(id);

            if (visitTypeFromRepo == null)
            {
                return NotFound();
            }

            _visitTypeRepository.DeleteVisitType(visitTypeFromRepo);
            await _visitTypeRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateVisitType(int id, VisitTypeForUpdateDto visitType)
        {
            var visitTypeFromRepo = await _visitTypeRepository.GetVisitTypeAsync(id);

            if (visitTypeFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new VisitTypeForUpdateDtoValidator().Validate(visitType);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(visitType, visitTypeFromRepo);
            _visitTypeRepository.UpdateVisitType(visitTypeFromRepo);

            await _visitTypeRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateVisitType(int id, JsonPatchDocument<VisitTypeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingVisitType = await _visitTypeRepository.GetVisitTypeAsync(id);

            if (existingVisitType == null)
            {
                return NotFound();
            }

            var visitTypeToPatch = _mapper.Map<VisitTypeForUpdateDto>(existingVisitType); // map the tipoVisita we got from the database to an updatable tipoVisita model
            patchDoc.ApplyTo(visitTypeToPatch, ModelState); // apply patchdoc updates to the updatable tipoVisita

            if (!TryValidateModel(visitTypeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(visitTypeToPatch, existingVisitType); // apply updates from the updatable tipoVisita to the db entity so we can apply the updates to the database
            _visitTypeRepository.UpdateVisitType(existingVisitType); // apply business updates to data if needed

            await _visitTypeRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
