using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IRatingRepository
    {

        Task AddRecipeRatingAsync(RecipeRating recipeRating);
        Task<RecipeRating?> GetCustomerRatingByIdAsync(Guid customerId, Guid recipeId);
        Task<ICollection<RecipeRating>> GetAllRecipeRatingAsync();
        Task<bool> IsExist(Guid customeerId, Guid recipeId);
        Task<double?> GetAverageRatingAsync(Guid recipeId);
        void DeleteRecipeRating(RecipeRating recipeRating);
        void UpdatRecipeRating(RecipeRating recipeRating);
    }
}
