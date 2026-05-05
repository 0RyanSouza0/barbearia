using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IBarbeiroRepository : IRepositoryBase<Barbeiro>
{
    Task<List<Barbeiro>> GetByEspecialidade(string especialidade);
    Task SaveChangesAsync();
}
