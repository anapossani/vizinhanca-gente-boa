using Microsoft.IdentityModel.Tokens;
using System; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vizinhanca.API.Models;

namespace Vizinhanca.API.Services
{
    public class TokenService
    {
        private readonly SymmetricSecurityKey _key; 
        private readonly string _issuer;
        private readonly string _audience;


        public TokenService(IConfiguration configuration)
        {

            var secretKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("A chave secreta do JWT (Jwt:Key) não está configurada.");
            }
            
            _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("O emissor do JWT (Jwt:Issuer) não está configurado.");
            _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("A audiência do JWT (Jwt:Audience) não está configurada.");
            
        }

        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                ]),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _issuer, 
                Audience = _audience,
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}