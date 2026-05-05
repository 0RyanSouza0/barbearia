using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IAgendamentoRepository : IRepositoryBase<Agendamento>
{
    Task<Agendamento> GetByClienteAsync(int id);
}
