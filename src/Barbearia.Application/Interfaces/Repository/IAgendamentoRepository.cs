using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IAgendamentoRepository : IRepositoryBase<Agendamento>
{
    Task SaveChangesAsync();
    Task<IList<Agendamento>> GetByClienteAsync(string nome);
}
