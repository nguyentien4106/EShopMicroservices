using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Domain.Models;
using BuildingBlocks.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Helpers;

public static class TokenUtils
{
    public static async Task<string> GenerateAccessToken(UserManager<User> userManager, JwtSettings appSettings, User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var userRoles = await userManager.GetRolesAsync(user);

        // Add role claims
        var claims = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
        
        var userClaims = new List<Claim>
        {
            new("userId", user.Id.ToString()),
            new ("userName", user.UserName ?? ""),
            new("email", user.Email ?? "")
        };
        
        userClaims.AddRange(claims);
        
        var tokenOptions = new JwtSecurityToken(
            issuer: appSettings.Issuer,
            audience: appSettings.Audience,
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(appSettings.AccessTokenLifetimeMinutes),
            signingCredentials: signInCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public static async Task<string> GenerateRefreshToken(UserManager<User> userManager, JwtSettings jwtSettings, User user)
    {
        const string refreshTokenKey = "RefreshToken";
        await userManager.RemoveAuthenticationTokenAsync(user, jwtSettings.RefreshTokenProvider, refreshTokenKey);
        var refreshToken = await userManager.GenerateUserTokenAsync(user, jwtSettings.RefreshTokenProvider, refreshTokenKey);
        await userManager.SetAuthenticationTokenAsync(user, jwtSettings.RefreshTokenProvider, refreshTokenKey, refreshToken);
        
        return refreshToken;
    }
}