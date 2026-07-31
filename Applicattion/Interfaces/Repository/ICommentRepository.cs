using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface ICommentRepository
    {

        Task AddRecipeCommentAsync(RecipeComment recipeComment);
        Task<RecipeComment?> GetRecipeCommentByIdAsync(Guid id);
        Task<ICollection<RecipeComment>> GetAllRecipeCommentAsync();
        void DeleteRecipeComment(RecipeComment recipeComment);
        void UpdateRecipeComment(RecipeComment recipeComment);
    }
}
