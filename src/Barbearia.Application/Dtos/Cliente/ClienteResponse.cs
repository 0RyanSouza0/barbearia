namespace Barbearia.Application.Dtos.Cliente;

public record ClienteResponse
{
    public int Id { get; init; }
    public string Nome { get; init; }
    public string Telefone { get; init; }
    public string Email { get; init; }
}
