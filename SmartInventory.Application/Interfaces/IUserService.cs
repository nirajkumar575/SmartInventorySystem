using SmartInventory.Application.DTOs.User;

namespace SmartInventory.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, UpdateUserDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
