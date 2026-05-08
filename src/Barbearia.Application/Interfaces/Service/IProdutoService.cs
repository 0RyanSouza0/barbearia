using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Produto;
using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Service;

public interface IProdutoService
{
    Task<Result<ProdutoResponse>> CreateAsync(ProdutoRequest request);
    Task<Result<ProdutoResponse>> UpdateAsync(int id, ProdutoUpdate update);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<ProdutoResponse>> GetByIdAsync(int id);
    Task<Result<ProdutoResponse>> GetProdutoByNome(string nome);
    Task<Result<IList<ProdutoResponse>>> GetProdutosByCategoria(Categoria? categoria);
    Task<Result<IEnumerable<ProdutoResponse>>> GetAllAsync();
}
