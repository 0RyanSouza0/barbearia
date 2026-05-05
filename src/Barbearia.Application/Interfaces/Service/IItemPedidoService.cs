using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.ItemPedido;

namespace Barbearia.Application.Interfaces.Service;

public interface IItemPedidoService
{
    Task<Result<ItemPedidoResponse>> CreateAsync(ItemPedidoRequest request);
    Task<Result<ItemPedidoResponse>> UpdateAsync(int id, ItemPedidoRequest request);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<ItemPedidoResponse>> GetByIdAsync(int id);
    Task<Result<IEnumerable<ItemPedidoResponse>>> GetAllAsync();


}
