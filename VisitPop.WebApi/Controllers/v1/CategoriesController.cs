using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Category;
using VisitPop.Application.Interfaces;
using VisitPop.Application.Validation.Category;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.WebApi.Controllers.v1
{
    /// <summary>
    /// Perform acceso CRUD For manipulate Categories Entitie
    /// </summary>
    [ApiController]
    [Route("api/Categories")]
    [ApiVersion("1.0")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository ??
                throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a List of Categories Entity
        /// </summary>
        /// <param name="categoryParametersDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetCategories")]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryParametersDto categoryParametersDto)
        {
            var categoryFromRepo = await _categoryRepository.GetCategoriesAsync(categoryParametersDto);

            var paginationMetaData = new
            {
                totalCount = categoryFromRepo.TotalCount,
                pageSize = categoryFromRepo.PageSize,
                currentPageSize = categoryFromRepo.CurrentPageSize,
                currentStartIndex = categoryFromRepo.CurrentStartIndex,
                currentEndIndex = categoryFromRepo.CurrentEndIndex,
                pageNumber = categoryFromRepo.PageNumber,
                totalPages = categoryFromRepo.TotalPages,
                hasPrevious = categoryFromRepo.HasPrevious,
                hasNext = categoryFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetaData));

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categoryFromRepo);
            var response = new Response<IEnumerable<CategoryDto>>(categoriesDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var categoryFromRepo = await _categoryRepository.GetCategoryAsync(id);

            if (categoryFromRepo == null)
                return NotFound();

            var categoryDto = _mapper.Map<CategoryDto>(categoryFromRepo);
            var response = new Response<CategoryDto>(categoryDto);

            return Ok(response);
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CategoryForCreationDto categoryForCreation)
        {
            var validationResults = new CategoryForCreationDtoValidator().Validate(categoryForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var category = _mapper.Map<Category>(categoryForCreation);
            await _categoryRepository.AddCategory(category);
            var saveSuccessful = await _categoryRepository.SaveAsync();

            if (saveSuccessful)
            {
                var categoryFromRepo = await _categoryRepository.GetCategoryAsync(category.Id);
                var categoryDto = _mapper.Map<CategoryDto>(categoryFromRepo);
                var response = new Response<CategoryDto>(categoryDto);

                return CreatedAtRoute("GetCategory",
                    new { categoryDto.Id },
                    response);
            }

            return StatusCode(500);

        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var categoryFromRepo = await _categoryRepository.GetCategoryAsync(id);

            if (categoryFromRepo == null)
                return NotFound();

            _categoryRepository.DeleteCategory(categoryFromRepo);
            await _categoryRepository.SaveAsync();

            return NoContent();
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryForUpdateDto category)
        {
            var categoryFromRepo = await _categoryRepository.GetCategoryAsync(id);

            if (categoryFromRepo == null)
                return NotFound();

            var validationResults = new CategoryForUpdateDtoValidator().Validate(category);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(category, categoryFromRepo);
            _categoryRepository.UpdateCategory(categoryFromRepo);

            await _categoryRepository.SaveAsync();

            return NoContent();
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> PartiallyUpdateCategory(int categoryId, JsonPatchDocument<CategoryForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var existingCategory = await _categoryRepository.GetCategoryAsync(categoryId);

            if (existingCategory == null)
                return NotFound();

            // map the category we got from the database to an updatable category model
            var categoryToPatch = _mapper.Map<CategoryForUpdateDto>(existingCategory);

            // apply patchdoc updates to the updatable category
            patchDoc.ApplyTo(categoryToPatch, ModelState);

            if (!TryValidateModel(categoryToPatch))
                return ValidationProblem(ModelState);

            // apply updates from the updatable category to the db entity so we can apply the updates to the database
            _mapper.Map(categoryToPatch, existingCategory);

            // apply business updates to data if needed
            _categoryRepository.UpdateCategory(existingCategory);

            // save changes in the database
            await _categoryRepository.SaveAsync();

            return NoContent();
        }

    }
}
