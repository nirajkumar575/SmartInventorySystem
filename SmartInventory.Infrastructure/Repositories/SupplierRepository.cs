using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Supplier?> GetByEmailAsync(string email)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(x => x.Email == email);
    }
}