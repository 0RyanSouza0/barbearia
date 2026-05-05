namespace Barbearia.Application.Dtos.Servicos;

public record ServicosRequest
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public int DuracaoMinutos { get; set; }
}
