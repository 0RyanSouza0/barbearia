using Barbearia.Domain.Enuns;

namespace Barbearia.Application.Interfaces.Auth;

public interface ITokenGenerator
{
    Task<string> GenerateToken(int id, string email, string role);
}
