using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Interfaces.RegisterControl;
using VisitPop.Application.Validation.RegisterControl;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/RegisterControls")]
    [ApiVersion("1.0")]
    public class RegisterControlsController : Controller
    {
        private readonly IRegisterControlRepository _registerControlRepository;
        private readonly IMapper _mapper;

        public RegisterControlsController(IRegisterControlRepository registerControlRepository,
            IMapper mapper)
        {
            _registerControlRepository = registerControlRepository
                ?? throw new ArgumentNullException(nameof(registerControlRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetRegisterControls")]
        public async Task<IActionResult> GetRegisterControls([FromQuery] RegisterControlParametersDto registerControlParameters)
        {
            var registerControlsFromRepo = await _registerControlRepository.GetRegisterControlsAsync(registerControlParameters);

            var paginationMetadata = new
            {
                totalCount = registerControlsFromRepo.MetaData.TotalCount,
                pageSize = registerControlsFromRepo.MetaData.PageSize,
                currentPageSize = registerControlsFromRepo.MetaData.CurrentPageSize,
                currentStartIndex = registerControlsFromRepo.MetaData.CurrentStartIndex,
                currentEndIndex = registerControlsFromRepo.MetaData.CurrentEndIndex,
                pageNumber = registerControlsFromRepo.MetaData.PageNumber,
                totalPages = registerControlsFromRepo.MetaData.TotalPages,
                hasPrevious = registerControlsFromRepo.MetaData.HasPrevious,
                hasNext = registerControlsFromRepo.MetaData.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var puntoControlesDto = _mapper.Map<IEnumerable<RegisterControlDto>>(registerControlsFromRepo);
            var response = new Response<IEnumerable<RegisterControlDto>>(puntoControlesDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetRegisterControl")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<RegisterControlDto>> GetRegisterControl(int id)
        {
            var registerControlFromRepo = await _registerControlRepository.GetRegisterControlAsync(id);

            if (registerControlFromRepo == null)
            {
                return NotFound();
            }

            var registerControlDto = _mapper.Map<RegisterControlDto>(registerControlFromRepo);
            var response = new Response<RegisterControlDto>(registerControlDto);

            return Ok(response);
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<RegisterControlDto>> AddRegisterControl([FromBody] RegisterControlForCreationDto registerControlForCreation)
        {
            var validatioResults = new RegisterControlForCreationDtoValidator().Validate(registerControlForCreation);
            validatioResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var registerControl = _mapper.Map<RegisterControl>(registerControlForCreation);
            await _registerControlRepository.AddRegisterControl(registerControl);
            var saveSuccessful = await _registerControlRepository.SaveAsync();

            if (saveSuccessful)
            {
                var registerControlFromRepo = await _registerControlRepository.GetRegisterControlAsync(registerControl.Id);
                var registerControlDto = _mapper.Map<RegisterControlDto>(registerControlFromRepo);
                var response = new Response<RegisterControlDto>(registerControlDto);

                return CreatedAtRoute("GetRegisterControl",
                    new { registerControlDto.Id },
                    response);
            }

            return StatusCode(500);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteRegisterControl(int id)
        {
            var registerControlFromRepo = await _registerControlRepository.GetRegisterControlAsync(id);

            if (registerControlFromRepo == null)
            {
                return NotFound();
            }

            _registerControlRepository.DeleteRegisterControl(registerControlFromRepo);
            await _registerControlRepository.SaveAsync();

            return NoContent();
        }


        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateRegisterControl(int id, RegisterControlForUpdateDto registerControl)
        {
            var registerControlFromRepo = await _registerControlRepository.GetRegisterControlAsync(id);

            if (registerControlFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new RegisterControlForUpdateDtoValidator().Validate(registerControl);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(registerControl, registerControlFromRepo);
            _registerControlRepository.UpdateRegisterControl(registerControlFromRepo);

            await _registerControlRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PartiallyUpdateRegisterControl(int id, JsonPatchDocument<RegisterControlForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingRegisterControl = await _registerControlRepository.GetRegisterControlAsync(id);

            if (existingRegisterControl == null)
            {
                return NotFound();
            }

            var registerControlToPatch = _mapper.Map<RegisterControlForUpdateDto>(existingRegisterControl); // map the puntoControl we got from the database to an updatable puntoControl model
            patchDoc.ApplyTo(registerControlToPatch, ModelState); // apply patchdoc updates to the updatable puntoControl

            if (!TryValidateModel(registerControlToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(registerControlToPatch, existingRegisterControl); // apply updates from the updatable puntoControl to the db entity so we can apply the updates to the database
            _registerControlRepository.UpdateRegisterControl(existingRegisterControl); // apply business updates to data if needed

            await _registerControlRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}
