using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IItemPedidoRepository : IRepositoryBase<ItemPedido>
{
    Task SaveChangesAsync();
}
