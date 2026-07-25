using SmartInventory.Domain.Entities;

namespace SmartInventory.Domain.Interfaces;

public interface ISupplierRepository : IGenericRepository<Supplier>
{
    Task<Supplier?> GetByEmailAsync(string email);
}