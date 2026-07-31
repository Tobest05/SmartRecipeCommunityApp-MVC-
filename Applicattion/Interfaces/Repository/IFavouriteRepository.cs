using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IFavouriteRecipeRepository
    {
        Task AddFavouriteRecipeAsync(Favourite favouriteRecipe);
        Task<Favourite?> GetFavouriteRecipeByIdAsync(Guid id);
        Task<bool> IsExist(Guid customeerId, Guid recipeId);
        Task<ICollection<Favourite>> GetAllFavouriteRecipeAsync();
        void DeleteFavouriteRecipe(Favourite favouriteRecipe);
    }
}