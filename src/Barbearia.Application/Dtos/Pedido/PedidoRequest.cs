using Barbearia.Application.Dtos.ItemPedido;

namespace Barbearia.Application.Dtos.Pedido;

public record PedidoRequest
{
    public int ClienteId { get; set; }
    public List<ItemPedidoRequest> Itens { get; set; }
}
