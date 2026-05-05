namespace Barbearia.Application.Dtos.Barbeiro;

public record BarbeiroRequest
{
    public string Nome { get; set; }
    public string Especialidade { get; set; }
}
