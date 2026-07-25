using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Common;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Infrastructure.Repositories
{
    public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext context) : base(context){ }

        public async Task<Purchase?> GetPurchaseWithItemsAsync(int id)
        {
            return await _context.Purchases.Include(x => x.Supplier).Include(x => x.PurchaseItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PagedResult<Purchase>> GetPagedPurchasesAsync(PurchaseQueryParameters request)
        {
            var query = _context.Purchases
                .Include(x => x.Supplier)
                .AsQueryable();

            // Search by Invoice Number
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.InvoiceNumber.Contains(request.Search));
            }

            // Supplier Filter
            if (request.SupplierId.HasValue)
            {
                query = query.Where(x =>
                    x.SupplierId == request.SupplierId.Value);
            }

            // Date Filter
            if (request.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.PurchaseDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.PurchaseDate <= request.ToDate.Value);
            }

            query = request.SortBy?.ToLower() switch
            {
                "invoice" => request.SortOrder == "desc"
                    ? query.OrderByDescending(x => x.InvoiceNumber)
                    : query.OrderBy(x => x.InvoiceNumber),

                "date" => request.SortOrder == "desc"
                    ? query.OrderByDescending(x => x.PurchaseDate)
                    : query.OrderBy(x => x.PurchaseDate),

                "amount" => request.SortOrder == "desc"
                    ? query.OrderByDescending(x => x.TotalAmount)
                    : query.OrderBy(x => x.TotalAmount),

                _ => query.OrderByDescending(x => x.Id)
            };

            var totalRecords = await query.CountAsync();

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Purchase>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        public async Task<Purchase?> GetPurchaseForUpdateAsync(int id)
        {
            return await _context.Purchases
                .Include(x => x.PurchaseItems)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<decimal> GetTotalPurchaseAmountAsync()
        {
            return await _context.Purchases
                .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;
        }

        public async Task<decimal> GetTodayPurchasesAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Purchases
                .Where(x => x.PurchaseDate.Date == today)
                .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;
        }
        public async Task<IEnumerable<Purchase>> GetPurchaseReportAsync(ReportQueryParameters request)
        {
            var query = _context.Purchases
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseItems)
                    .ThenInclude(x => x.Product)
                .AsQueryable();

            if (request.FromDate.HasValue)
            {
                query = query.Where(x => x.PurchaseDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(x => x.PurchaseDate <= request.ToDate.Value);
            }

            return await query
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetRecentPurchasesAsync(int count)
        {
            return await _context.Purchases
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.PurchaseDate)
                .Take(count)
                .ToListAsync();
        }
        public async Task<IEnumerable<Purchase>> GetLast7DaysPurchasesAsync()
        {
            var fromDate = DateTime.UtcNow.Date.AddDays(-6);

            return await _context.Purchases
                .Where(x => x.PurchaseDate >= fromDate)
                .OrderBy(x => x.PurchaseDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products
                .Where(x => x.Quantity <= threshold)
                .OrderBy(x => x.Quantity)
                .ToListAsync();
        }
        public async Task<IEnumerable<DashboardChartData>> GetLast7DaysPurchaseChartAsync()
        {
            var fromDate = DateTime.UtcNow.Date.AddDays(-6);

            return await _context.Purchases
                .Where(x => x.PurchaseDate >= fromDate)
                .GroupBy(x => x.PurchaseDate.Date)
                .Select(g => new DashboardChartData
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
    }
}
