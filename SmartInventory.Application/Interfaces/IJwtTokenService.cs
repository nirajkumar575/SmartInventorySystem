using SmartInventory.Domain.Entities;

namespace SmartInventory.Application.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateToken(ApplicationUser user);
}