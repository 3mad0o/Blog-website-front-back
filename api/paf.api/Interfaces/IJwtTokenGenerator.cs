using Microsoft.AspNetCore.Identity;

namespace paf.api.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IdentityUser identityUser,IList<string> Roles);
    }
}
