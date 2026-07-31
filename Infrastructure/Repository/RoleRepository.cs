using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SmartRecipeContext _context;

    public RoleRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Role
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _context.Role
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Role>> GetAllRoleAsync()
    {
        return await _context.Role
            .ToListAsync();
    }
}

