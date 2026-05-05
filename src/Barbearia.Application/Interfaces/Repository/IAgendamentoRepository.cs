using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IAgendamentoRepository : IRepositoryBase<Agendamento>
{
    Task SaveChangesAsync();
    Task<Agendamento> GetByClienteAsync(string nome);
}
