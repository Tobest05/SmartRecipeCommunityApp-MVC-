using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InstructionRepository : IInstructionRepository
{
    private readonly SmartRecipeContext _context;

    public InstructionRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddInstructionAsync(Instruction instruction)
    {
        await _context.Instruction.AddAsync(instruction);
    }

    public async Task<Instruction?> GetInstructionByIdAsync(Guid id)
    {
        return await _context.Instruction
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Instruction>> GetAllInstructionAsync()
    {
        return await _context.Instruction
            .Include(x => x.Recipe)
            .ToListAsync();
    }

    public void DeleteInstruction(Instruction instruction)
    {
        _context.Instruction.Remove(instruction);
    }

    public void UpdateInstruction(Instruction instruction)
    {
        _context.Instruction.Update(instruction);
    }
}
