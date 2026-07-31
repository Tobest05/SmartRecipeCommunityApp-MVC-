using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IInstructionRepository
    {

        Task AddInstructionAsync(Instruction instruction);
        Task<Instruction?> GetInstructionByIdAsync(Guid id);
        Task<ICollection<Instruction>> GetAllInstructionAsync();
        void DeleteInstruction(Instruction instruction);
        void UpdateInstruction(Instruction instruction);
    }
}
