using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetByCustomerIdAsync(Guid id);
        Task<ICollection<Customer>> GetAllCustomerAsync();
        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
    }
}
