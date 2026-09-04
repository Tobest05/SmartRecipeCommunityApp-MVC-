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

        Task<ICollection<Favourite>> GetFavouriteRecipeByCustomerIdAsync(
            Guid customerId);

        Task<bool> IsExist(Guid customerId, Guid recipeId);

        void DeleteFavouriteRecipe(Favourite favouriteRecipe);
    }
}