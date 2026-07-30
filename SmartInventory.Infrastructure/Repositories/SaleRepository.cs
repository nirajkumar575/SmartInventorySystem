using Microsoft.EntityFrameworkCore;
using SmartInventory.Application.DTOs.Dashboard;
using SmartInventory.Domain.Common;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Infrastructure.Repositories;

public class SaleRepository : GenericRepository<Sale>, ISaleRepository
{
    public SaleRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Sale?> GetSaleWithItemsAsync(int id)
    {
        return await _context.Sales
            .Include(x => x.Customer)
            .Include(x => x.SaleItems)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Sale?> GetSaleForUpdateAsync(int id)
    {
        return await _context.Sales
            .Include(x => x.SaleItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedResult<Sale>> GetPagedSalesAsync(
        SaleQueryParameters request)
    {
        IQueryable<Sale> query = _context.Sales
            .Include(x => x.Customer);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.InvoiceNumber.Contains(request.Search));
        }

        if (request.CustomerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == request.CustomerId);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(x =>
                x.Status == request.Status);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.SaleDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x =>
                x.SaleDate <= request.ToDate.Value);
        }

        // Sorting
        query = request.SortBy.ToLower() switch
        {
            "invoice" => request.Descending
                ? query.OrderByDescending(x => x.InvoiceNumber)
                : query.OrderBy(x => x.InvoiceNumber),

            "customer" => request.Descending
                ? query.OrderByDescending(x => x.Customer.Name)
                : query.OrderBy(x => x.Customer.Name),

            "totalamount" => request.Descending
                ? query.OrderByDescending(x => x.TotalAmount)
                : query.OrderBy(x => x.TotalAmount),

            _ => request.Descending
                ? query.OrderByDescending(x => x.SaleDate)
                : query.OrderBy(x => x.SaleDate)
        };

        var totalRecords = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<Sale>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }
    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _context.Sales
            .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;
    }

    public async Task<decimal> GetTodaySalesAsync()
    {
        var today = DateTime.UtcNow.Date;

        return await _context.Sales
            .Where(x => x.SaleDate.Date == today)
            .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;
    }

    public async Task<IEnumerable<Sale>> GetSalesReportAsync(ReportQueryParameters request)
    {
        var query = _context.Sales
            .Include(x => x.Customer)
            .Include(x => x.SaleItems)
                .ThenInclude(x => x.Product)
            .AsQueryable();

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.SaleDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.SaleDate <= request.ToDate.Value);
        }

        return await query
            .OrderByDescending(x => x.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetRecentSalesAsync(int count)
    {
        return await _context.Sales
            .Include(x => x.Customer)
            .OrderByDescending(x => x.SaleDate)
            .Take(count)
            .ToListAsync();
    }
    public async Task<IEnumerable<TopSellingProduct>> GetTopSellingProductsAsync(int count)
    {
        return await _context.SaleItems
            .Include(x => x.Product)
            .GroupBy(x => new
            {
                x.ProductId,
                x.Product.Name
            })
            .Select(g => new TopSellingProduct
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                QuantitySold = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.QuantitySold)
            .Take(count)
            .ToListAsync();
    }
    public async Task<IEnumerable<Sale>> GetLast7DaysSalesAsync()
    {
        var fromDate = DateTime.UtcNow.Date.AddDays(-6);

        return await _context.Sales
            .Where(x => x.SaleDate >= fromDate)
            .OrderBy(x => x.SaleDate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<DashboardChartData>> GetLast7DaysSalesChartAsync()
    {
        var fromDate = DateTime.UtcNow.Date.AddDays(-6);

        return await _context.Sales
            .Where(x => x.SaleDate >= fromDate)
            .GroupBy(x => x.SaleDate.Date)
            .Select(g => new DashboardChartData
            {
                Date = g.Key,
                TotalAmount = g.Sum(x => x.TotalAmount)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<Sale?> GetInvoiceAsync(int saleId)
    {
        return await _context.Sales
            .Include(x => x.Customer)
            .Include(x => x.SaleItems)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == saleId);
    }
}