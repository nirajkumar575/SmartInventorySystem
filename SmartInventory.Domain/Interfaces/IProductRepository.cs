using SmartInventory.Domain.Entities;

namespace SmartInventory.Domain.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku);
}