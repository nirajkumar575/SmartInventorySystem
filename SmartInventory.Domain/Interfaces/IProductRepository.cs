using SmartInventory.Domain.Entities;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Domain.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku);
    Task<PagedResult<Product>> GetPagedProductsAsync(ProductQueryParameters request);
    Task<int> GetLowStockCountAsync(int threshold);
    Task<IEnumerable<Product>> GetStockReportAsync();
}