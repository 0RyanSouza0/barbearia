using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Agendamento;

namespace Barbearia.Application.Interfaces.Service;

public interface IAgendamentoService
{
    Task<Result<AgendamentoResponse>> CreateAysnc(AgendamentoRequest request);
    Task<Result<IEnumerable<AgendamentoResponse>>> GetAllAsync();
    Task<Result<AgendamentoResponse>> GetByIdAsync(int id);
    Task<Result<AgendamentoResponse>> GetByClienteAsync(string nome);
    Task<Result<AgendamentoResponse>> UpdateAsync(int id, AgendamentoRequest request);
    Task<Result<string>> DeleteAsync(int id);

}
