using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SmartRecipeContext _context;

    public UserRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.User.AddAsync(user);
    }

    public async Task<bool?> IsExistAsync(string email)
    {
        return await _context.User.AnyAsync(x => x.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.User
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetBIdAsync(Guid id)
    {
        return await _context.User
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<User>> GetAllUserAsync()
    {
        return await _context.User
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .ToListAsync();
    }

    public void DeleteUser(User user)
    {
        _context.User.Remove(user);
    }

    public void UpdateUser(User user)
    {
        _context.User.Update(user);
    }
}
