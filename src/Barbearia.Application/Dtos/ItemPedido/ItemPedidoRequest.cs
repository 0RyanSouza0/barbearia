namespace Barbearia.Application.Dtos.ItemPedido;

public record ItemPedidoRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }


}
