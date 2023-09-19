using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    // https:localhost:PORT/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private ApplicationDbContext dbContext;
        public CategoriesController(ApplicationDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryrequestDto request)
        {
            var category = new Category
            {
                // id is added by entity framework
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var response = new CategoryDto { Id = category.Id, Name = request.Name, UrlHandle = category.UrlHandle };
            return Ok(response);
        }
    }
}
