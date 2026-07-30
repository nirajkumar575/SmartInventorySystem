using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? Browser { get; set; }
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
}