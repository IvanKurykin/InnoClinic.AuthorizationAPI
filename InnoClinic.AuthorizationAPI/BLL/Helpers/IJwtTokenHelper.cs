using System.Diagnostics.CodeAnalysis;

namespace BLL.Helpers;

public interface IJwtTokenHelper
{
    string GenerateJwtToken(string email, string role);
}
