using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class PedidoRepository(AppDbContext _context) : IPedidoRepository
{
    async Task<Pedido> IRepositoryBase<Pedido>.Add(Pedido entity)
    {
        var pedido = await _context.Pedidos.AddAsync(entity);
        return pedido.Entity;
    }

    Task IRepositoryBase<Pedido>.Delete(Pedido entity)
    {
        var pedido = _context.Pedidos.Remove(entity);
        return Task.CompletedTask;
    }

    async Task<IEnumerable<Pedido>> IRepositoryBase<Pedido>.GetAll()
    {
        var pedidos = await _context.Pedidos.ToListAsync();
        return pedidos;
    }

    async Task<Pedido> IRepositoryBase<Pedido>.GetById(int id)
    {
        var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
        return pedido;
    }

    Task IPedidoRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Pedido> IRepositoryBase<Pedido>.Update(Pedido entity)
    {
        _context.Pedidos.Update(entity);
        return Task.FromResult(entity);
    }
}
