using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CodePulse.API.Controllers
{
    // https:localhost:PORT/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository _categoryRepository)
        {
            this.categoryRepository = _categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryrequestDto request)
        {
            var category = new Category
            {
                // id is added by entity framework
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);

            var response = new CategoryDto { Id = category.Id, Name = request.Name, UrlHandle = category.UrlHandle };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            var response = new List<CategoryDto>();

            foreach(var cat in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    UrlHandle = cat.UrlHandle
                }); ;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.GetById(id);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto updatedCategory)
        {
            var category = new Category
            {
                Id = id,
                Name = updatedCategory.Name,
                UrlHandle = updatedCategory.UrlHandle,
            };

            category = await categoryRepository.UpdateAsync(category);
            if (category is null)
            {
                return NotFound();
            }

            var response = new Category
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);

        }
    }
}
