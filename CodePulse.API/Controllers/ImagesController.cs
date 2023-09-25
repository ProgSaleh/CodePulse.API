using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileName = fileName,
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                blogImage = await _imageRepository.Upload(file, blogImage);

                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    FileName = blogImage.FileName,
                    FileExtension = blogImage.FileExtension,
                    Title = blogImage.Title,
                    url = blogImage.url,
                    DateCreated = blogImage.DateCreated,
                };
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var blogImages = await _imageRepository.GetAllAsync();
            var response = new List<BlogImageDto>();

            foreach(var img in blogImages)
            {
                response.Add(new BlogImageDto
                {
                    Id = img.Id,
                    FileName = img.FileName,
                    FileExtension = img.FileExtension,
                    Title = img.Title,
                    url = img.url,
                    DateCreated = img.DateCreated,
                });
            }

            return Ok(response);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format!");
            }

            if (file.Length > 10_485_760)
            {
                ModelState.AddModelError("file", "Exceeded max file size 10MB");
            }
        }
    }
}
