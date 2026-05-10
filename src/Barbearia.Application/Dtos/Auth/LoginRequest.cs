namespace Barbearia.Application.Dtos.Auth;

public record LoginRequest
{
    public string Email { get; set; }
    public string Senha { get; set; }
}
