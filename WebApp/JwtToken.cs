using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TshisrtApiService;

namespace WebApp
{
    public static class JwtToken
    {
        public static string Create(string userName)
        {
            if(!string.IsNullOrWhiteSpace(userName))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            else
            {
                return new string("");
            }
        }
    }
}
