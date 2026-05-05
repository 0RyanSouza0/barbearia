using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Produto;

namespace Barbearia.Application.Interfaces.Service;

public interface IProdutoService
{
    Task<Result<ProdutoResponse>> CreateAsync(ProdutoRequest request);
    Task<Result<ProdutoResponse>> UpdateAsync(int id, ProdutoRequest request);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<ProdutoResponse>> GetByIdAsync(int id);
    Task<Result<ProdutoResponse>> GetProdutoByIdNome(string nome);
    Task<Result<IEnumerable<ProdutoResponse>>> GetAllAsync();
}
