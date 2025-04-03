using DAL.Entities;

namespace BLL.Helpers;

public interface IJwtTokenHelper
{
    string GenerateJwtToken(User user, IList<string> roles);
}
