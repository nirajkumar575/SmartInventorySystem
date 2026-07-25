using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class CustomerRepository
    : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}