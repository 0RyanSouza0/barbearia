namespace Barbearia.Application.Dtos.Servicos;

public record ServicosUpdate
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal? Valor { get; set; }
    public int? DuracaoMinutos { get; set; }
}
