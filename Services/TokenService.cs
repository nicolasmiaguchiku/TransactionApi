using System.IdentityModel.Tokens.Jwt;
using TransactionsApi.Models;
using System.Text;
using TransactionsApi.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TransactionsApi.Services
{
    public static class TokenService
    {
        public static string GenerateToken(Client client)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(SecretKey.KeySecret);

            var credentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2),
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                    new Claim(ClaimTypes.Email, client.Email ?? ""),
                    new Claim(ClaimTypes.Name, client.Name ?? "")
                ])
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

    }
}
