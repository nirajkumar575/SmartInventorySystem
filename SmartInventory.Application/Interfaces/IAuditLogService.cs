using SmartInventory.Application.DTOs.Audit;

namespace SmartInventory.Application.Interfaces;

public interface IAuditLogService
{
    Task AddAsync(string module, string action, string description);
    Task<List<AuditLogDto>> GetAllAsync();
}