using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly SmartRecipeContext _context;

    public CustomerRepository(SmartRecipeContext context)
    {
        _context = context;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _context.Customer.AddAsync(customer);
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customer
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Customer?> GetByCustomerIdAsync(Guid id)
    {
        return await _context.Customer
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<ICollection<Customer>> GetAllCustomerAsync()
    {
        return await _context.Customer
            .Include(x => x.User)
            .ToListAsync();
    }

    public void DeleteCustomer(Customer customer)
    {
        _context.Customer.Remove(customer);
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customer.Update(customer);
    }
}
