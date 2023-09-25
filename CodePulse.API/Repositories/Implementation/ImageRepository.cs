using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository: IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public ImageRepository (IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccesor, ApplicationDbContext applicationDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccesor;
            _dbContext = applicationDbContext;
        }
        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var request = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{request.Scheme}://{request.Host}{request.PathBase}/images/{blogImage.FileName}{blogImage.FileExtension}";
            
            blogImage.url = urlPath;

            await _dbContext.BlogImages.AddAsync(blogImage);
            await _dbContext.SaveChangesAsync();

            return blogImage;
        }

        public async Task<IEnumerable<BlogImage>> GetAllAsync()
        {
            return await _dbContext.BlogImages.ToListAsync();
        }
    }
}
