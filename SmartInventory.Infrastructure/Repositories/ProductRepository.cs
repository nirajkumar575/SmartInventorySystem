using Microsoft.EntityFrameworkCore;
using SmartInventory.Shared.QueryParameters;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;
using SmartInventory.Shared.Common;

namespace SmartInventory.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.SKU == sku);
    }

    public async Task<PagedResult<Product>> GetPagedProductsAsync(ProductQueryParameters request)
    {
        var query = _context.Products.Include(x => x.Category).AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.Name.Contains(request.Search) ||
                x.SKU.Contains(request.Search));
        }

        // Price Filter
        if (request.MinPrice.HasValue)
        {
            query = query.Where(x => x.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(x => x.Price <= request.MaxPrice.Value);
        }

        // Sorting
        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortOrder == "desc"
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),

            "price" => request.SortOrder == "desc"
                ? query.OrderByDescending(x => x.Price)
                : query.OrderBy(x => x.Price),

            "quantity" => request.SortOrder == "desc"
                ? query.OrderByDescending(x => x.Quantity)
                : query.OrderBy(x => x.Quantity),

            _ => query.OrderBy(x => x.Id)
        };

        var totalRecords = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<Product>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }

    public async Task<int> GetLowStockCountAsync(int threshold)
    {
        return await _context.Products
            .CountAsync(x => x.Quantity <= threshold);
    }
}