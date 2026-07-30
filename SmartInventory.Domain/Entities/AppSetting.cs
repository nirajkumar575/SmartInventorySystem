using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class AppSetting : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogo { get; set; }
    public string GSTNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Website { get; set; }
}