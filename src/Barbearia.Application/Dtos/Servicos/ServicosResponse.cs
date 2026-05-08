namespace Barbearia.Application.Dtos.Servicos;

public record ServicosResponse
{
    public int Id { get; init; }
    public string Nome { get; init; }
    public string Descricao { get; init; }
    public decimal Valor { get; init; }
    public int DuracaoMinutos { get; init; }
}
