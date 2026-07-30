namespace SmartInventory.Application.DTOs.Role;

using System.ComponentModel.DataAnnotations;

public class UserRoleDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string RoleName { get; set; } = string.Empty;
}