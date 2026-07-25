using SmartInventory.Domain.Entities;

namespace SmartInventory.Domain.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}