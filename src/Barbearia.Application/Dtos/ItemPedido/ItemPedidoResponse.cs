namespace Barbearia.Application.Dtos.ItemPedido;

public record ItemPedidoResponse
{
    public int Id { get; init; }
    public int PedidoId { get; init; }
    public int ProdutoId { get; init; }
    public int Quantidade { get; init; }
    public decimal ValorUnitario { get; init; }
}
