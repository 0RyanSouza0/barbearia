namespace Barbearia.Application.Dtos.Administrador;

public record AdministradorRequest
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
}
