using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IUserRoleRepository
    {

        Task AddUserRoleAsync(UserRole userRole);
        Task<bool?> IsExist(Guid userId, Guid roleId);
        Task<ICollection<UserRole>> GetAllUserRoleAsync();
        Task<UserRole?> GetByIdAsync(Guid userId);
        void DeleteUserRole(UserRole userRole);
    }
}
