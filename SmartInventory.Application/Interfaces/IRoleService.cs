using SmartInventory.Application.DTOs.Role;

namespace SmartInventory.Application.Interfaces;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    Task<bool> CreateAsync(CreateRoleDto dto);
    Task<bool> DeleteAsync(string roleName);
    Task<bool> AssignRoleAsync(UserRoleDto dto);
}