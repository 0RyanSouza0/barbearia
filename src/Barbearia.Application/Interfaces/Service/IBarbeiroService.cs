using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Barbeiro;

namespace Barbearia.Application.Interfaces.Service;

public interface IBarbeiroService
{
    Task<Result<BarbeiroResponse>> CreateAsync(BarbeiroRequest request);
    Task<Result<BarbeiroResponse>> UpdateAsync(int id, BarbeiroRequest request);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<BarbeiroResponse>> GetByIdAsync(int id);
    Task<Result<IEnumerable<BarbeiroResponse>>> GetAllAsync();
    Task<Result<List<BarbeiroResponse>>> GetByEspecialidadeAsync(string especialidade);
}
