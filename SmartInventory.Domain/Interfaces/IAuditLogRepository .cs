using SmartInventory.Domain.Entities;

namespace SmartInventory.Domain.Interfaces;

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    Task<List<AuditLog>> GetAllAsync();
}