using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RecipeRatingRepository : IRatingRepository
{
    private readonly SmartRecipeContext _context;

    public RecipeRatingRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddRecipeRatingAsync(RecipeRating recipeRating)
    {
        await _context.RecipeRatings.AddAsync(recipeRating);
    }

    public async Task<RecipeRating?> GetCustomerRatingByIdAsync(Guid customerId, Guid recipeId)
    {
        return await _context.RecipeRatings
            .Include(x => x.Customer)
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(x =>
                x.CustomerId == customerId &&
                x.RecipeId == recipeId);
    }

    public async Task<ICollection<RecipeRating>> GetAllRecipeRatingAsync()
    {
        return await _context.RecipeRatings
            .Include(x => x.Customer)
            .Include(x => x.Recipe)
            .ToListAsync();
    }

    public async Task<bool> IsExist(Guid customerId, Guid recipeId)
    {
        return await _context.RecipeRatings
            .AnyAsync(x =>
                x.CustomerId == customerId &&
                x.RecipeId == recipeId);
    }

    public async Task<double?> GetAverageRatingAsync(Guid recipeId)
    {
        var ratings = await _context.RecipeRatings
            .Where(x => x.RecipeId == recipeId)
            .ToListAsync();

        if (!ratings.Any())
        {
            return null;
        }

        return ratings.Average(x => x.Rating);
    }

    public void DeleteRecipeRating(RecipeRating recipeRating)
    {
        _context.RecipeRatings.Remove(recipeRating);
    }

    public void UpdatRecipeRating(RecipeRating recipeRating)
    {
        _context.RecipeRatings.Update(recipeRating);
    }
}
