using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Cliente;

namespace Barbearia.Application.Interfaces.Service;

public interface IClienteService
{
    Task<Result<ClienteResponse>> CreateAsync(ClienteRequest request);
    Task<Result<ClienteResponse>> UpdateAsync(int id, ClienteUpdate update);
    Task<Result<ClienteResponse>> GetByIdAsync(int id);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<IEnumerable<ClienteResponse>>> GetAllAsync();
    Task<Result<IEnumerable<ClienteResponse>>> GetAllRelatorioAsync();
    Task<Result<ClienteResponse>> GetByEmailAsync(string email);
}
