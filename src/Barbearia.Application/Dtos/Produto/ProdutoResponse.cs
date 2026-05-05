namespace Barbearia.Application.Dtos.Produto;

public record ProdutoResponse
{
    public string Nome { get; init; }
    public decimal Valor { get; init; }
    public int Estoque { get; init; }
    public string Descricao { get; init; }
    public bool Ativo { get; init; }
    public int CategoriaId { get; init; }
}
