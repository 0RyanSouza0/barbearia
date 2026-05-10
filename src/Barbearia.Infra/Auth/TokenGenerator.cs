using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Barbearia.Application.Interfaces.Auth;
using Barbearia.Domain.Enuns;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Barbearia.Infra.Auth;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    async Task<string> ITokenGenerator.GenerateToken(int id, string email, string role)
    {
        var claims = new[]
        {
            new Claim (ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var tempoExpiracao = _configuration.GetValue<int>("Jwt:ExpirationMinutes");
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tempoExpiracao),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
