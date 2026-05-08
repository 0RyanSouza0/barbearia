using Barbearia.Application.Dtos.ItemPedido;

namespace Barbearia.Application.Dtos.Pedido;

public record PedidoUpdate
{
    public List<ItemPedidoUpdate>? Itens { get; set; }
}
