using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IProdutoRepository : IRepositoryBase<Produto>
{
    Task<Produto> GetProdutoByNameAsync(string nome);
}
