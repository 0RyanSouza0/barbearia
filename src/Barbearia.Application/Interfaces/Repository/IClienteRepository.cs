using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IClienteRepository : IRepositoryBase<Cliente>
{
    Task<Cliente> GetByEmailAsync(string email);
}
