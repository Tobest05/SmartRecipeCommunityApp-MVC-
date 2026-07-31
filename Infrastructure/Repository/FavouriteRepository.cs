using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FavouriteRecipeRepository : IFavouriteRecipeRepository
{
    private readonly SmartRecipeContext _context;

    public FavouriteRecipeRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddFavouriteRecipeAsync(Favourite favouriteRecipe)
    {
        await _context.FavouriteRecipes.AddAsync(favouriteRecipe);
    }

    public async Task<Favourite?> GetFavouriteRecipeByIdAsync(Guid id)
    {
        return await _context.FavouriteRecipes
            .Include(x => x.Customer)
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> IsExist(Guid customerId, Guid recipeId)
    {
        return await _context.FavouriteRecipes
            .AnyAsync(x => x.CustomerId == customerId &&
                           x.RecipeId == recipeId);
    }

    public async Task<ICollection<Favourite>> GetAllFavouriteRecipeAsync()
    {
        return await _context.FavouriteRecipes
            .Include(x => x.Customer)
            .Include(x => x.Recipe)
            .ToListAsync();
    }

    public void DeleteFavouriteRecipe(Favourite favouriteRecipe)
    {
        _context.FavouriteRecipes.Remove(favouriteRecipe);
    }
}
