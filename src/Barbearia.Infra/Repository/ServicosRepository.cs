using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class ServicosRepository(AppDbContext _context) : IServicosRepository
{
    async Task<Servicos> IRepositoryBase<Servicos>.Add(Servicos entity)
    {
        var servicos = await _context.Servicos.AddAsync(entity);
        return servicos.Entity;
    }

    Task IRepositoryBase<Servicos>.Delete(Servicos entity)
    {
        _context.Servicos.Remove(entity);
        return Task.CompletedTask;
    }

    async Task<IEnumerable<Servicos>> IRepositoryBase<Servicos>.GetAll()
    {
        var servicos = await _context.Servicos.ToListAsync();
        return servicos;
    }

    Task<Servicos> IRepositoryBase<Servicos>.GetById(int id)
    {
        var servicos = _context.Servicos.FirstOrDefaultAsync(s => s.Id == id);
        return servicos;
    }

    Task<Servicos> IServicosRepository.GetByName(string name)
    {
        var servicos = _context.Servicos.FirstOrDefaultAsync(s => s.Nome == name);
        return servicos;
    }

    Task IServicosRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Servicos> IRepositoryBase<Servicos>.Update(Servicos entity)
    {
        _context.Servicos.Update(entity);
        return Task.FromResult(entity);
    }
}
