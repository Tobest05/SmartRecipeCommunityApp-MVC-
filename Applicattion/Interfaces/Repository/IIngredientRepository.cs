using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IIngredientRepository
    {

        Task AddIngredientAsync(Ingredient ingredient);
        Task<Ingredient?> GetIngredientByIdAsync(Guid id);
        Task<bool> IsExist(Guid recipeId, string name);
        Task<ICollection<Ingredient>> GetAllIngredientAsync();
        void DeleteIngredient(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
    }
}
