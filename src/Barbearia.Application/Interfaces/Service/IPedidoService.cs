using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Pedido;

namespace Barbearia.Application.Interfaces.Service;

public interface IPedidoService
{
    Task<Result<PedidoResponse>> CreateAsync(PedidoRequest request);
    Task<Result<IEnumerable<PedidoResponse>>> GetAllAsync();
    Task<Result<PedidoResponse>> GetByIdAsync(int id);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<PedidoResponse>> AddNovoItem(int id, AdicionarItemRequest itemRequest);
    Task<Result<PedidoResponse>> UpdateQuantidadeItemPedido(int id, AtualizarQuantidadeItemRequest quantidadeItemRequest);

    Task<Result<PedidoResponse>> RemoverItemPedido(int id, RemoverItemPedidoRequest itemRequest);
}
