using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.Helpers.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Helpers;

[ExcludeFromCodeCoverage]
public class JwtTokenHelper : IJwtTokenHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenHelper(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwtToken(string email, string role)
    {
        ValidateSecretKey(_jwtSettings.SecretKey ?? throw new InvalidOperationException("JWT Secret Key is not configured"));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = CreateTokenDescriptor(email, key, role);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public static void ValidateSecretKey(string secretKey)
    {
        if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            throw new InvalidOperationException();
    }

    public static IEnumerable<Claim> CreateClaims(string email, string role)
    {
        var claims = new List<Claim>
        { 
            new Claim(JwtRegisteredClaimNames.Sub, email),  
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        return claims;
    }
    public SecurityTokenDescriptor CreateTokenDescriptor(string email, byte[] key, string role)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(CreateClaims(email, role)),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _jwtSettings.Issuer ?? throw new InvalidOperationException(),
            Audience = _jwtSettings.Audience ?? throw new InvalidOperationException()
        };
    }
}
