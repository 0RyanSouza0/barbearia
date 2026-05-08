using Barbearia.Application.Dtos.ItemPedido;

namespace Barbearia.Application.Dtos.Pedido;

public record AtualizarQuantidadeItemRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
