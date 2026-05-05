using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Pedido;

namespace Barbearia.Application.Interfaces.Service;

public interface IPedidoService
{
    Task<Result<PedidoResponse>> CreateAsync(PedidoRequest request);
    Task<Result<IEnumerable<PedidoResponse>>> GetAllAsync();
    Task<Result<PedidoResponse>> GetByIdAsync(int id);
    Task<Result<PedidoResponse>> UpdateAsync(int id, PedidoRequest request);
    Task<Result<string>> DeleteAsync(int id);
}
