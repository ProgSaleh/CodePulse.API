using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository (ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost post)
        {
            await _dbContext.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(post => post.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BlogPosts.Include(post => post.Categories).FirstOrDefaultAsync(post => post.Id == id);

        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingPost = await _dbContext.BlogPosts.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == blogPost.Id);
            if (existingPost is not null)
            {
                _dbContext.Entry(existingPost).CurrentValues.SetValues(blogPost);
                existingPost.Categories = blogPost.Categories;
                await _dbContext.SaveChangesAsync();
                return blogPost;
            }
            else
            {
                return null;
            }
        }
    }
}
