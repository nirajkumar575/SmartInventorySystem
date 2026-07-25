using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<int> GetTotalCategoriesAsync()
    {
        return await _context.Categories.CountAsync();
    }

    public async Task<int> GetLowStockProductsAsync()
    {
        return await _context.Products.CountAsync(x => x.Quantity <= 10);
    }

    public async Task<int> GetOutOfStockProductsAsync()
    {
        return await _context.Products.CountAsync(x => x.Quantity == 0);
    }

    public async Task<decimal> GetTotalInventoryValueAsync()
    {
        return await _context.Products.SumAsync(x => x.Price * x.Quantity);
    }
}