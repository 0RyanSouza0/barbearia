using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class ItemPedidoRepository(AppDbContext _context) : IItemPedidoRepository
{
    async Task<ItemPedido> IRepositoryBase<ItemPedido>.Add(ItemPedido entity)
    {
        var itemPedido = await _context.ItensPedidos.AddAsync(entity);
        return itemPedido.Entity;
    }

    Task IRepositoryBase<ItemPedido>.Delete(ItemPedido entity)
    {
        _context.ItensPedidos.Remove(entity);
        return Task.CompletedTask;
    }

    async Task<IEnumerable<ItemPedido>> IRepositoryBase<ItemPedido>.GetAll()
    {
        var itemPedidos = await _context.ItensPedidos.ToListAsync();
        return itemPedidos;
    }

    async Task<ItemPedido> IRepositoryBase<ItemPedido>.GetById(int id)
    {
        var itemPedido = await _context.ItensPedidos.FirstOrDefaultAsync(i => i.Id == id);
        return itemPedido;
    }

    Task IItemPedidoRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<ItemPedido> IRepositoryBase<ItemPedido>.Update(ItemPedido entity)
    {
        _context.ItensPedidos.Update(entity);
        return Task.FromResult(entity);
    }
}
