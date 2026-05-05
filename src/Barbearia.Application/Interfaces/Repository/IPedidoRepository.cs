using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IPedidoRepository : IRepositoryBase<Pedido>
{
    Task SaveChangesAsync();
}
