using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class ProdutoRepository(AppDbContext _context) : IProdutoRepository
{
    async Task<Produto> IRepositoryBase<Produto>.Add(Produto entity)
    {
        var produto = await _context.Produtos.AddAsync(entity);
        return produto.Entity;
    }

    Task IRepositoryBase<Produto>.Delete(Produto entity)
    {
        throw new NotImplementedException();
    }

    async Task<IEnumerable<Produto>> IRepositoryBase<Produto>.GetAll()
    {
        var produtos = await _context.Produtos.ToListAsync();
        return produtos;
    }

    async Task<IEnumerable<Produto>> IProdutoRepository.GetAllProdutosRelatorio()
    {
        var produtos = await _context.Produtos.IgnoreQueryFilters().AsNoTracking().ToListAsync();
        return produtos;
    }

    async Task<Produto?> IRepositoryBase<Produto>.GetById(int id)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
        return produto;
    }

    async Task<IList<Produto>> IProdutoRepository.GetProdutoByCategoria(Categoria categoria)
    {
        return await _context.Produtos.Where(p => p.Categoria == categoria).ToListAsync();
    }

    async Task<Produto?> IProdutoRepository.GetProdutoByNameAsync(string nome)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == nome);
        return produto;
    }

    Task IProdutoRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Produto> IRepositoryBase<Produto>.Update(Produto entity)
    {
        _context.Produtos.Update(entity);
        return Task.FromResult(entity);
    }
}
