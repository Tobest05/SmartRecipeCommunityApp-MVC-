using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly SmartRecipeContext _context;

    public UserRoleRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddUserRoleAsync(UserRole userRole)
    {
        await _context.UserRole.AddAsync(userRole);
    }

    public async Task<bool?> IsExist(Guid userId, Guid roleId)
    {
        return await _context.UserRole
    .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
    }

    public async Task<ICollection<UserRole>> GetAllUserRoleAsync()
    {
        return await _context.UserRole
            .Include(x => x.User)
            .Include(x => x.Role)
            .ToListAsync();
    }
    public async Task<UserRole?> GetByIdAsync(Guid userId)
    {
        return await _context.UserRole
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public void DeleteUserRole(UserRole userRole)
    {
        _context.UserRole.Remove(userRole);
    }
}
