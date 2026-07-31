using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{

    public interface IRecipeRepository
    {
        Task<Recipe?> GetByIdAsync(Guid id);
        Task<ICollection<Recipe>> GetAllAsync();
        Task<ICollection<Recipe>> GetPublishedRecipesAsync();
        Task<ICollection<Recipe>> GetByCustomerIdAsync(Guid customerId);
        Task<ICollection<Recipe>> SearchByNameAsync(string name);
        Task<ICollection<Recipe>> GetByCategoryIdAsync(Guid categoryId);
        Task<ICollection<Recipe>> GetByDifficultyAsync(Difficulty difficulty);
        Task<ICollection<Recipe>> GetByStatusAsync(RecipeStatus status);
        Task<ICollection<Recipe>> GetByCookingTimeAsync(int maxCookingTime);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Recipe recipe);
        void Update(Recipe recipe);
        void Delete(Recipe recipe);
    }
}

