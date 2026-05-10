using Barbearia.Domain.Entities;

namespace Barbearia.Application.Dtos.Produto;

public record ProdutoResponse
{
    public int Id { get; init; }
    public string Nome { get; init; }
    public decimal Valor { get; init; }
    public int Estoque { get; init; }
    public string Descricao { get; init; }
    public DateTime DataCriacao { get; init; }
    public DateTime DataAtualizacao { get; init; }
    public bool Ativo { get; init; }
    public Categoria Categoria { get; init; }
}
