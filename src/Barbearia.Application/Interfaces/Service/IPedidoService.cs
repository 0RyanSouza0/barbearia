using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Pedido;

namespace Barbearia.Application.Interfaces.Service;

public interface IPedidoService
{
    Task<Result<PedidoResponse>> CreateAsync(PedidoRequest request);
    Task<Result<IEnumerable<PedidoResponse>>> GetAllAsync();
    Task<Result<PedidoResponse>> GetByIdAsync(int id);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<PedidoResponse>> AddItem(int id, PedidoUpdate pedidoUpdate);
    Task<Result<PedidoResponse>> UpdateQuantidade(int id, PedidoUpdate pedidoUpdate);
}
