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
        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
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
            };

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
            };

            return Ok(response);
        }
    }
}
