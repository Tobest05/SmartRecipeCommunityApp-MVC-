using Application.Interfaces.Repository;
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly SmartRecipeContext _context;

    public RecipeRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task<Recipe?> GetByIdAsync(Guid id)
    {
        return await _context.Recipe
            .Include(x => x.Category)
            .Include(x => x.Customer)
            .Include(x => x.Ingredients)
            .Include(x => x.Instruction)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Recipe>> GetAllAsync()
    {
        return await _context.Recipe
            .Include(x => x.Category)
            .Include(x => x.Customer)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> GetPublishedRecipesAsync()
    {
        return await _context.Recipe
            .Where(x => x.Status == RecipeStatus.Published)
            .Include(x => x.Category)
            .Include(x => x.Customer)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Recipe
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> SearchByNameAsync(string name)
    {
        return await _context.Recipe
            .Where(x => x.Name.Contains(name))
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Recipe
            .Where(x => x.CategoryId == categoryId)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> GetByDifficultyAsync(Difficulty difficulty)
    {
        return await _context.Recipe
            .Where(x => x.Difficulty == difficulty)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> GetByStatusAsync(RecipeStatus status)
    {
        return await _context.Recipe
            .Where(x => x.Status == status)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<ICollection<Recipe>> GetByCookingTimeAsync(int maxCookingTime)
    {
        return await _context.Recipe
            .Where(x => x.CookingTimeMinutes <= maxCookingTime)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Recipe
            .AnyAsync(x => x.Id == id);
    }

    public async Task AddAsync(Recipe recipe)
    {
        await _context.Recipe.AddAsync(recipe);
    }

    public void Update(Recipe recipe)
    {
        _context.Recipe.Update(recipe);
    }

    public void Delete(Recipe recipe)
    {
        _context.Recipe.Remove(recipe);
    }
}
