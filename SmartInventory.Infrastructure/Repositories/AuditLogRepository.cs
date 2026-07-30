using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext context) : base(context) { }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _context.AuditLogs.OrderByDescending(x => x.ActionDate).ToListAsync();
    }
}