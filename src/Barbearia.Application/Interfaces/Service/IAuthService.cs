using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Auth;

namespace Barbearia.Application.Interfaces.Service;

public interface IAuthService
{
    Task<Result<string>> LoginAdminAsync(LoginRequest login);
    Task<Result<string>> LoginClienteAsync(LoginRequest login);
}
