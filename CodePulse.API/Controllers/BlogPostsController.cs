using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {

        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto req)
        {
            var blogPost = new BlogPost
            {
                Title = req.Title,
                ShortDescription = req.ShortDescription,
                Content = req.Content,
                FeaturedImageUrl = req.FeaturedImageUrl,
                UrlHandle = req.UrlHandle,
                PublishedDate = req.PublishedDate,
                Author = req.Author,
                IsVisible = req.IsVisible,
                Categories = new List<Category>(),
            };

            foreach(var categoryGuid in req.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryGuid);
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await _blogPostRepository.CreateAsync(blogPost);

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(cat => new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    UrlHandle = cat.UrlHandle,
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();
            var response = new List<BlogPostDto>();

            foreach(var post in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    ShortDescription = post.ShortDescription,
                    Content = post.Content,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    UrlHandle = post.UrlHandle,
                    PublishedDate = post.PublishedDate,
                    Author = post.Author,
                    IsVisible = post.IsVisible,
                    Categories = post.Categories.Select(cat => new CategoryDto
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        UrlHandle = cat.UrlHandle,
                    }).ToList()
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var existingBlogPost = await _blogPostRepository.GetByIdAsync(id);
            if (existingBlogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = existingBlogPost.Id,
                Title = existingBlogPost.Title,
                ShortDescription = existingBlogPost.ShortDescription,
                Content = existingBlogPost.Content,
                FeaturedImageUrl = existingBlogPost.FeaturedImageUrl,
                UrlHandle = existingBlogPost.UrlHandle,
                PublishedDate = existingBlogPost.PublishedDate,
                Author = existingBlogPost.Author,
                IsVisible = existingBlogPost.IsVisible,
                Categories = existingBlogPost.Categories?.Select(cat => new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    UrlHandle = cat.UrlHandle,
                }).ToList(),

            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditBlogPost([FromRoute] Guid id, UpdateBlogPostRequestDto post)
        {
            var updatedPost = new BlogPost
            {
                Id = id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                FeaturedImageUrl = post.FeaturedImageUrl,
                UrlHandle = post.UrlHandle,
                PublishedDate = post.PublishedDate,
                Author = post.Author,
                IsVisible = post.IsVisible,
                Categories = new List<Category>()
            };

            foreach(var catId in post.Categories)
            {
                var existingCat = await _categoryRepository.GetById(catId);
                if (existingCat is not null)
                {
                    updatedPost.Categories.Add(existingCat);
                }
            }

            updatedPost = await _blogPostRepository.UpdateAsync(updatedPost);

            if (updatedPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = updatedPost.Id,
                Title = updatedPost.Title,
                ShortDescription = updatedPost.ShortDescription,
                Content = updatedPost.Content,
                FeaturedImageUrl = updatedPost.FeaturedImageUrl,
                UrlHandle = updatedPost.UrlHandle,
                PublishedDate = updatedPost.PublishedDate,
                Author = updatedPost.Author,
                IsVisible = updatedPost.IsVisible,
                Categories = updatedPost.Categories.Select(cat => new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    UrlHandle = cat.UrlHandle,
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedPost = await _blogPostRepository.DeleteAsync(id);
            if (deletedPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = deletedPost.Id,
                Title = deletedPost.Title,
                ShortDescription = deletedPost.ShortDescription,
                Content = deletedPost.Content,
                FeaturedImageUrl = deletedPost.FeaturedImageUrl,
                UrlHandle = deletedPost.UrlHandle,
                PublishedDate = deletedPost.PublishedDate,
                Author = deletedPost.Author,
                IsVisible = deletedPost.IsVisible
            };

            return Ok(response);
        }
    }
}
