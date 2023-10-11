using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using paf.api.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace paf.api.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration configuration;

        public JwtTokenGenerator(IConfiguration configuration )
        {
            this.configuration = configuration;
        }
        public string GenerateToken(IdentityUser identityUser,IList<string> Roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,identityUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName,identityUser.UserName),

            };
            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
            var cred =new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
            var TokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = cred,
                Expires = DateTime.Now.AddDays(7),
                Issuer = configuration["Token:Issuer"],


            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var Token =TokenHandler.CreateToken(TokenDescripter);
            return TokenHandler.WriteToken(Token);

        }
    }
}
