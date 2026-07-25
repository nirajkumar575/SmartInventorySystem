using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartInventory.Application.DTOs.Auth;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Data;
using SmartInventory.Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartInventory.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public JwtTokenService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> userManager,
        AppDbContext context)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _context = context;
    }
    public async Task<LoginResponseDto> GenerateTokensAsync(ApplicationUser user)
    {
        var accessToken = await GenerateAccessToken(user);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            Expires = GetRefreshTokenExpiry(),
            ApplicationUserId = user.Id
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),

            User = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                UserName = user.UserName!
            }
        };
    }
    public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .Include(x => x.ApplicationUser)
            .FirstOrDefaultAsync(x =>
                x.Token == refreshToken &&
                !x.IsRevoked);

        if (token == null)
            return null;

        if (token.IsExpired)
            return null;

        token.IsRevoked = true;
        token.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            Expires = GetRefreshTokenExpiry(),
            ApplicationUserId = token.ApplicationUserId
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        await _context.SaveChangesAsync();

        var accessToken = await GenerateAccessToken(token.ApplicationUser);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),

            User = new UserDto
            {
                Id = token.ApplicationUser.Id,
                FullName = token.ApplicationUser.FullName,
                Email = token.ApplicationUser.Email!,
                UserName = token.ApplicationUser.UserName!
            }
        };
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x =>
                x.Token == refreshToken);

        if (token == null)
            return;

        token.IsRevoked = true;
        token.RevokedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
    private async Task<string> GenerateAccessToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Email, user.Email!),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

        new(ClaimTypes.NameIdentifier, user.Id),
        new(ClaimTypes.Name, user.UserName!),
        new(ClaimTypes.Email, user.Email!)
    };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            + Guid.NewGuid().ToString("N");
    }
    private DateTime GetRefreshTokenExpiry()
    {
        return DateTime.UtcNow.AddDays(7);
    }
}