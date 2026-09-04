using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RecipeCommentRepository : ICommentRepository
{
    private readonly SmartRecipeContext _context;

    public RecipeCommentRepository(SmartRecipeContext context)
    {
        _context = context;
    }


    public async Task AddRecipeCommentAsync(
        RecipeComment recipeComment)
    {
        await _context.RecipeComments.AddAsync(recipeComment);
    }


    public async Task<RecipeComment?> GetRecipeCommentByIdAsync(
        Guid id)
    {
        return await _context.RecipeComments

            .Include(x => x.Customer)

            .Include(x => x.Recipe)

            .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<ICollection<RecipeComment>>
        GetAllRecipeCommentAsync()
    {
        return await _context.RecipeComments

            .Include(x => x.Customer)

            .Include(x => x.Recipe)

            .ToListAsync();
    }


    public async Task<ICollection<RecipeComment>>
        GetRecipeCommentByRecipeIdAsync(
            Guid recipeId)
    {
        return await _context.RecipeComments

            .Where(x => x.RecipeId == recipeId)

            .Include(x => x.Customer)

            .Include(x => x.Recipe)

            .OrderByDescending(x => x.CreatedBy)

            .ToListAsync();
    }


    public async Task<ICollection<RecipeComment>>
        GetRecipeCommentByCustomerIdAsync(
            Guid customerId)
    {
        return await _context.RecipeComments

            .Where(x => x.CustomerId == customerId)

            .Include(x => x.Customer)

            .Include(x => x.Recipe)

            .OrderByDescending(x => x.CreatedBy)

            .ToListAsync();
    }


    public void DeleteRecipeComment(
        RecipeComment recipeComment)
    {
        _context.RecipeComments.Remove(recipeComment);
    }


    public void UpdateRecipeComment(
        RecipeComment recipeComment)
    {
        _context.RecipeComments.Update(recipeComment);
    }
}
