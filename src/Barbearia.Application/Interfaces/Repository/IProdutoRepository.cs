using Barbearia.Domain.Entities;

namespace Barbearia.Application.Interfaces.Repository;

public interface IProdutoRepository : IRepositoryBase<Produto>
{
    Task SaveChangesAsync();
    Task<Produto> GetProdutoByNameAsync(string nome);
    Task<IList<Produto>> GetProdutoByCategoria(Categoria categoria);
    Task<IEnumerable<Produto>> GetAllProdutosRelatorio();
}
