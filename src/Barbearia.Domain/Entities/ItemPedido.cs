namespace Barbearia.Domain.Entities;

public class ItemPedido : BaseEntidade
{
    public ItemPedido(int pedidoId, int produtoId, int quantidade, decimal valorUnitario)
    {
        PedidoId = pedidoId;
        ProdutoId = produtoId;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }

    public void AumentarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.");

        Quantidade += quantidade;
    }
}
