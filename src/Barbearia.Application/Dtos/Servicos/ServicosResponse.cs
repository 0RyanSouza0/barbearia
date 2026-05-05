namespace Barbearia.Application.Dtos.Servicos;

public record ServicosResponse
{
    public string Nome { get; init; }
    public string Descricao { get; init; }
    public decimal Valor { get; init; }
    public int DuracaoMinutos { get; init; }
}
