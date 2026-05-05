using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IServicosRepository : IRepositoryBase<Servicos>
{
    Task SaveChangesAsync();
    Task<Servicos> GetByName(string name);
}
