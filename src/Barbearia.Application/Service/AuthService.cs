
using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Auth;
using Barbearia.Application.Interfaces.Auth;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;

namespace Barbearia.Application.Service;

public class AuthService(IClienteRepository clienteRepository, IAdministradorRepository administradorRepository, ITokenGenerator token) : IAuthService
{
    async Task<Result<string>> IAuthService.LoginAdminAsync(LoginRequest login)
    {
        var admin = await administradorRepository.GetByEmail(login.Email);
        if (admin is null || !BCrypt.Net.BCrypt.Verify(login.Senha, admin.Senha))
        {
            return Result<string>.Failure("Email ou senha inválidos");
        }

        var tokenGerado = await token.GenerateToken(admin.Id, admin.Email, admin.Role.ToString());
        return Result<string>.Success(tokenGerado);
    }

    async Task<Result<string>> IAuthService.LoginClienteAsync(LoginRequest login)
    {
        var cliente = await clienteRepository.GetByEmailAsync(login.Email);
        if (cliente is null || !BCrypt.Net.BCrypt.Verify(login.Senha, cliente.Senha))
        {
            return Result<string>.Failure("Email ou senha inválidos");
        }

        var tokenGerado = await token.GenerateToken(cliente.Id, cliente.Email, cliente.Role.ToString());
        return Result<string>.Success(tokenGerado);
    }
}
