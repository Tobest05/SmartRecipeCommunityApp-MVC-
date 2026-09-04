using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}

