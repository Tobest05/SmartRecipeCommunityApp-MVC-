using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly SmartRecipeContext _context;

    public CategoryRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _context.Category.AddAsync(category);
    }

    public async Task<Category?> GetByIdAsync(Guid userId)
    {
        return await _context.Category
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Category
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<bool?> IsExistAsync(string name)
    {
        return await _context.Category
            .AnyAsync(x => x.Name == name);
    }

    public async Task<ICollection<Category>> GetAllCategoryAsync()
    {
        return await _context.Category
            .ToListAsync();
    }

    public void UpdateCategory(Category category)
    {
        _context.Category.Update(category);
    }

    public void DeleteCategory(Category category)
    {
        _context.Category.Remove(category);
    }
}
