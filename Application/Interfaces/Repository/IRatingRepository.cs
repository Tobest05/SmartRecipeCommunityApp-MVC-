using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IRatingRepository
    {

        Task AddRecipeRatingAsync(RecipeRating recipeRating);
        Task<double?> GetAverageCustomerRatingByIdAsync(Guid customerId);
        Task<RecipeRating?> GetByCustomerAndRecipeAsync(Guid customerId, Guid recipeId);
        Task<ICollection<RecipeRating>> GetAllRecipeRatingAsync();
        Task<ICollection<RecipeRating>> GetByCustomerIdAsync(Guid customerId);
        Task<bool> IsExist(Guid customerId, Guid recipeId);
        Task<double?> GetAverageRatingAsync(Guid recipeId);
        void DeleteRecipeRating(RecipeRating recipeRating);
        void UpdatRecipeRating(RecipeRating recipeRating);
    }
}
