using Barbearia.Application.Dtos.Auth;
using Barbearia.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IAuthService authService) : ControllerBase
{
    [HttpPost("admin")]
    public async Task<IActionResult> LoginAdminAsync([FromBody] LoginRequest login)
    {
        var loginAdmin = await authService.LoginAdminAsync(login);
        if (loginAdmin.IsFailure)
        {
            return BadRequest(loginAdmin.Errors);
        }
        return Ok(loginAdmin.Value);
    }

    [HttpPost("cliente")]
    public async Task<IActionResult> LoginClienteAsync([FromBody] LoginRequest login)
    {
        var loginCliente = await authService.LoginClienteAsync(login);
        if (loginCliente.IsFailure)
        {
            return BadRequest(loginCliente.Errors);
        }
        return Ok(loginCliente.Value);
    }
}
