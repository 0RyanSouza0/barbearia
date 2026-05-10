using Barbearia.Domain.Enuns;

namespace Barbearia.Application.Dtos.Administrador;

public record AdministradorResponse
{
    public int Id { get; init; }
    public DateTime CriadoEm { get; init; }
    public DateTime AtualizadoEm { get; init; }
    public Role Role { get; init; }
    public string? Nome { get; init; }
    public string? Email { get; init; }
    public bool Ativo { get; init; }
    public string? Senha { get; init; }
}
