using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PuntoControl;
using VisitPop.Application.Interfaces.PuntoControl;
using VisitPop.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using VisitPop.Application.Validation.PuntoControl;
using FluentValidation.AspNetCore;
using VisitPop.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/PuntoControles")]
    [ApiVersion("1.0")]
    public class PuntoControlesController : Controller
    {
        private readonly IPuntoControlRepository _puntoControlRepository;
        private readonly IMapper _mapper;

        public PuntoControlesController(IPuntoControlRepository puntoControlRepository,
            IMapper mapper)
        {
            _puntoControlRepository = puntoControlRepository
                ?? throw new ArgumentNullException(nameof(puntoControlRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetPuntoControles")]
        public async Task<IActionResult> GetPuntoControles([FromQuery] PuntoControlParametersDto puntoControlParameters)
        {
            var puntoControlesFromRepo = await _puntoControlRepository.GetPuntoControlesAsync(puntoControlParameters);

            var paginationMetadata = new
            {
                totalCount = puntoControlesFromRepo.MetaData.TotalCount,
                pageSize = puntoControlesFromRepo.MetaData.PageSize,
                currentPageSize = puntoControlesFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = puntoControlesFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = puntoControlesFromRepo.MetaData.CurrentEndIndex,
                pageNumber = puntoControlesFromRepo.MetaData.PageNumber,
                totalPages = puntoControlesFromRepo.MetaData.TotalPages,
                hasPrevious = puntoControlesFromRepo.MetaData.HasPrevious,
                hasNext = puntoControlesFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var puntoControlesDto = _mapper.Map<IEnumerable<PuntoControlDto>>(puntoControlesFromRepo);
            var response = new Response<IEnumerable<PuntoControlDto>>(puntoControlesDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetPuntoControl")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PuntoControlDto>> GetPuntoControl(int id)
        {
            var puntoControlFromRepo = await _puntoControlRepository.GetPuntoControlAsync(id);

            if (puntoControlFromRepo == null)
            {
                return NotFound();
            }

            var puntoControlDto = _mapper.Map<PuntoControlDto>(puntoControlFromRepo);
            var response = new Response<PuntoControlDto>(puntoControlDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PuntoControlDto>> AddPuntoControl([FromBody] PuntoControlForCreationDto puntoControlForCreation)
        {
            var validatioResults = new PuntoControlForCreationDtoValidator().Validate(puntoControlForCreation);
            validatioResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var puntoControl = _mapper.Map<PuntoControl>(puntoControlForCreation);
            await _puntoControlRepository.AddPuntoControl(puntoControl);
            var saveSuccessful = await _puntoControlRepository.SaveAsync();

            if (saveSuccessful)
            {
                var puntoControlFromRepo = await _puntoControlRepository.GetPuntoControlAsync(puntoControl.Id);
                var puntoControlDto = _mapper.Map<PuntoControlDto>(puntoControlFromRepo);
                var response = new Response<PuntoControlDto>(puntoControlDto);

                return CreatedAtRoute("GetPuntoControl",
                    new { puntoControlDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeletePuntoControl(int id)
        {
            var puntoControlFromRepo = await _puntoControlRepository.GetPuntoControlAsync(id);

            if (puntoControlFromRepo == null)
            {
                return NotFound();
            }

            _puntoControlRepository.DeletePuntoControl(puntoControlFromRepo);
            await _puntoControlRepository.SaveAsync();

            return NoContent();
        }


        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePuntoControl(int id, PuntoControlForUpdateDto puntoControl)
        {
            var puntoControlFromRepo = await _puntoControlRepository.GetPuntoControlAsync(id);

            if (puntoControlFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new PuntoControlForUpdateDtoValidator().Validate(puntoControl);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(puntoControl, puntoControlFromRepo);
            _puntoControlRepository.UpdatePuntoControl(puntoControlFromRepo);

            await _puntoControlRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdatePuntoControl(int id, JsonPatchDocument<PuntoControlForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingPuntoControl = await _puntoControlRepository.GetPuntoControlAsync(id);

            if (existingPuntoControl == null)
            {
                return NotFound();
            }

            var puntoControlToPatch = _mapper.Map<PuntoControlForUpdateDto>(existingPuntoControl); // map the puntoControl we got from the database to an updatable puntoControl model
            patchDoc.ApplyTo(puntoControlToPatch, ModelState); // apply patchdoc updates to the updatable puntoControl

            if (!TryValidateModel(puntoControlToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(puntoControlToPatch, existingPuntoControl); // apply updates from the updatable puntoControl to the db entity so we can apply the updates to the database
            _puntoControlRepository.UpdatePuntoControl(existingPuntoControl); // apply business updates to data if needed

            await _puntoControlRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
