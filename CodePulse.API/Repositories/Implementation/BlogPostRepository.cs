using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;

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
    }
}
