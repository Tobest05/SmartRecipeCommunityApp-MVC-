using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task<Category?> GetByIdAsync(Guid userId);
        Task<Category?> GetByNameAsync(string name);
        Task<bool?> IsExistAsync(string name);
        Task<ICollection<Category>> GetAllCategoryAsync();
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);

    }
}
