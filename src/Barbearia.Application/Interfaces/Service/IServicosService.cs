using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Servicos;

namespace Barbearia.Application.Interfaces.Service;

public interface IServicosService
{
    Task<Result<ServicosResponse>> CreateAsync(ServicosRequest request);
    Task<Result<ServicosResponse>> UpdateAsync(int id, ServicosRequest request);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<IEnumerable<ServicosResponse>>> GetAllAsync();
    Task<Result<ServicosResponse>> GetByIdAsync(int id);
    Task<Result<ServicosResponse>> GetByIdNome(string nome);

}
