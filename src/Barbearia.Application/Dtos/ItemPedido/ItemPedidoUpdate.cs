namespace Barbearia.Application.Dtos.ItemPedido;

public record ItemPedidoUpdate
{
    public int? PedidoId { get; set; }
    public int? ProdutoId { get; set; }
    public int? Quantidade { get; set; }
    public decimal? ValorUnitario { get; set; }

}
