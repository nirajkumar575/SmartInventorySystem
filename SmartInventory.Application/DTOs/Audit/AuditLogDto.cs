namespace SmartInventory.Application.DTOs.Audit;

public class AuditLogDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? Browser { get; set; }
    public DateTime ActionDate { get; set; }
}