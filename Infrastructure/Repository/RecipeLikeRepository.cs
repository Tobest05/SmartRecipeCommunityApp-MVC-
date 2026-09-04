using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class RecipeLikeRepository : IRecipeLikeRepository
    {
        private readonly SmartRecipeContext _context;

        public RecipeLikeRepository(SmartRecipeContext context)
        {
            _context = context;
        }

        public async Task<RecipeLike?> GetAsync(
            Guid customerId,
            Guid recipeId)
        {
            return await _context.RecipeLikes
                .FirstOrDefaultAsync(x =>
                    x.CustomerId == customerId &&
                    x.RecipeId == recipeId);
        }

        public async Task AddAsync(RecipeLike like)
        {
            await _context.RecipeLikes.AddAsync(like);
        }

        public void Remove(RecipeLike like)
        {
            _context.RecipeLikes.Remove(like);
        }

        public async Task<int> GetLikeCountAsync(Guid recipeId)
        {
            return await _context.RecipeLikes
                .CountAsync(x => x.RecipeId == recipeId);
        }
    }
}
