using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Administrador;

namespace Barbearia.Application.Interfaces.Service;

public interface IAdministradorService
{
    Task<Result<AdministradorResponse>> CreateAsync(AdministradorRequest request);
    Task<Result<AdministradorResponse>> UpdateAsync(int id, AdministradorRequest request);
    Task<Result<AdministradorResponse>> GetByIdAsync(int id);
    Task<Result<AdministradorResponse>> GetByEmailAsync(string email);
    Task<Result<IEnumerable<AdministradorResponse>>> GetAllAsync();
    Task<Result<IEnumerable<AdministradorResponse>>> GetAllRelatorioAsync();
    Task<Result<string>> DeleteAsync(int id);
}
