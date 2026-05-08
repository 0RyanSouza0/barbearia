using Barbearia.Domain.Entities;

namespace Barbearia.Application.Dtos.Produto;

public record ProdutoUpdate
{

    public string? Nome { get; set; }
    public decimal? Valor { get; set; }
    public int? Estoque { get; set; }
    public string? Descricao { get; set; }
    public Categoria? Categoria { get; set; }

}
