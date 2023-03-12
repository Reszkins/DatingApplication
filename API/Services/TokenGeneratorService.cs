using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(string email);
    }
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public string GenerateToken(string email)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("$ecr3tKeeyYT11nd3r@pp"));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:8081",
                audience: "http://localhost:8081",
                claims: new List<Claim> { new Claim("userName", email) },
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
