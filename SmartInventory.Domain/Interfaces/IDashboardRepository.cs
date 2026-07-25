namespace SmartInventory.Domain.Interfaces;

public interface IDashboardRepository
{
    Task<int> GetTotalProductsAsync();
    Task<int> GetTotalCategoriesAsync();
    Task<int> GetLowStockProductsAsync();
    Task<int> GetOutOfStockProductsAsync();
    Task<decimal> GetTotalInventoryValueAsync();
}