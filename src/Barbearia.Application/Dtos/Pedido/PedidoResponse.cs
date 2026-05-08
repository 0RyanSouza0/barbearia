using Barbearia.Domain.Enuns;

namespace Barbearia.Application.Dtos.Pedido;

public record PedidoResponse
{
    public int Id { get; init; }
    public DateTime Data { get; init; }
    public decimal ValorTotal { get; init; }
    public StatusPedido Status { get; init; }
    public IEnumerable<ItemPedido.ItemPedidoResponse> itemPedidos;
    public int IdCliente { get; init; }
}
