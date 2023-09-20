using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid categoryId)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == categoryId);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var categoryToUpdate = await dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == category.Id);
            if (categoryToUpdate is not null)
            {
                dbContext.Entry(categoryToUpdate).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            else
            {
                return null;
            }
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == id);
            if (category is null)
            {
                return null;
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
    }
}
