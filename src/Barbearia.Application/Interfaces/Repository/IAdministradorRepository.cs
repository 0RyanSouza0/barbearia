using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IAdministradorRepository : IRepositoryBase<Administrador>
{
    Task SaveChangesAsync();
    Task<IEnumerable<Administrador>> GetAllAdministradoresRelatorio();
    Task<Administrador> GetByEmail(string email);
}
