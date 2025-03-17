namespace BuildingBlocks.Auth.Models;

public class JwtSettings
{
    public string SecretKey { get; set; } = default!;
    
    public string Issuer { get; set; } = default!;  
    
    public string Audience { get; set; } = default!;
    
    public string Authority { get; set; } = default!;
    
    public int AccessTokenLifetimeMinutes { get; set; } = 10;

    public int RefreshTokenLifetimeDays { get; set; } = 10;
    public string RefreshTokenProvider { get; set; } = "Default";
}