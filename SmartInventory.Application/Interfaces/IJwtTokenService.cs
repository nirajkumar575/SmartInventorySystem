using SmartInventory.Application.DTOs.Auth;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Application.Interfaces;

public interface IJwtTokenService
{
    Task<LoginResponseDto> GenerateTokensAsync(ApplicationUser user);
    Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}