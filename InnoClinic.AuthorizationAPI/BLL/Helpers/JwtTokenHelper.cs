using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.Helpers.Settings;
using DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Helpers;

public class JwtTokenHelper : IJwtTokenHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenHelper(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwtToken(User user, IList<string> roles)
    {
        ValidateSecretKey(_jwtSettings.SecretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = CreateTokenDescriptor(user, key, roles);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public void ValidateSecretKey(string secretKey)
    {
        if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            throw new InvalidOperationException();
    }

    public IEnumerable<Claim> CreateClaims(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        { 
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),  
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    public SecurityTokenDescriptor CreateTokenDescriptor(User user, byte[] key, IList<string> roles)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(CreateClaims(user, roles)),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };
    }
}
