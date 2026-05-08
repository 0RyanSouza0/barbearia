using Barbearia.Domain.Enuns;

namespace Barbearia.Domain.Entities;

public class Pedido : BaseEntidade
{
    public Pedido(int clienteId)
    {
        ClienteId = clienteId;
        Status = StatusPedido.Criado;
        _itens = new List<ItemPedido>();
        Ativo = true;
    }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public bool Ativo { get; set; }
    public StatusPedido Status { get; set; }
    public decimal ValorTotal => _itens.Sum(i => i.Quantidade * i.ValorUnitario);
    private readonly List<ItemPedido> _itens;
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    public void AdicionarItem(int produtoId, int quantidade, decimal valorUnitario)
    {
        if (Status == StatusPedido.Cancelado || Status == StatusPedido.Enviado)
            throw new InvalidOperationException("Pedido não pode ser alterado");

        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (itemExistente != null)
        {
            itemExistente.AumentarQuantidade(quantidade);
            return;
        }

        _itens.Add(new ItemPedido(Id, produtoId, quantidade, valorUnitario));
    }

    public void AtualizarQuantidadeItem(int produtoId, int novaQuantidade)
    {
        if (Status == StatusPedido.Cancelado || Status == StatusPedido.Enviado)
            throw new InvalidOperationException("Pedido não pode ser alterado");

        if (novaQuantidade <= 0)
            throw new InvalidOperationException("Quantidade deve ser maior que zero");

        var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (item is null)
            throw new InvalidOperationException("Item não encontrado no pedido");

        item.AumentarQuantidade(novaQuantidade);
    }

    public void RemoverItem(int produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (item != null)
            _itens.Remove(item);
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Enviado)
            throw new InvalidOperationException("Pedido já enviado não pode ser cancelado.");
        if (Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Pedido ja cancelado.");

        Status = StatusPedido.Cancelado;
    }
}