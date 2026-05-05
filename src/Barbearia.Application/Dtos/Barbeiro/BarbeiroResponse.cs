namespace Barbearia.Application.Dtos.Barbeiro;

public record BarbeiroResponse
{
    public int Id { get; init; }
    public string Nome { get; init; }
    public string Especialidade { get; init; }

}
