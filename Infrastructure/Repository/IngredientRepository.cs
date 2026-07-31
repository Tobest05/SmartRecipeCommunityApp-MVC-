using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly SmartRecipeContext _context;

    public IngredientRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddIngredientAsync(Ingredient ingredient)
    {
        await _context.Ingredient.AddAsync(ingredient);
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(Guid id)
    {
        return await _context.Ingredient
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> IsExist(Guid recipeId, string name)
    {
        return await _context.Ingredient
            .AnyAsync(x => x.RecipeId == recipeId && x.Name == name);
    }

    public async Task<ICollection<Ingredient>> GetAllIngredientAsync()
    {
        return await _context.Ingredient
            .Include(x => x.Recipe)
            .ToListAsync();
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        _context.Ingredient.Remove(ingredient);
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        _context.Ingredient.Update(ingredient);
    }
}