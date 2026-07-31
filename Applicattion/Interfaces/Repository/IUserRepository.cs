using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<bool?> IsExistAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetBIdAsync(Guid id);
        Task<ICollection<User>> GetAllUserAsync();
        void DeleteUser(User user);
        void UpdateUser(User user);
    }
}
