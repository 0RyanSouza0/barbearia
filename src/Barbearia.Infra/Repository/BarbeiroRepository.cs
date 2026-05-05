using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class BarbeiroRepository : IBarbeiroRepository
{
    private readonly AppDbContext _context;

    public BarbeiroRepository(AppDbContext context)
    {
        _context = context;
    }
    async Task<Barbeiro> IRepositoryBase<Barbeiro>.Add(Barbeiro entity)
    {
        var barbeiro = await _context.Barbeiros.AddAsync(entity);
        return barbeiro.Entity;
    }

    Task IRepositoryBase<Barbeiro>.Delete(Barbeiro entity)
    {
        _context.Barbeiros.Remove(entity);
        return Task.CompletedTask;

    }

    async Task<IEnumerable<Barbeiro>> IRepositoryBase<Barbeiro>.GetAll()
    {
        var barbeiros = await _context.Barbeiros.ToListAsync();
        return barbeiros;
    }

    async Task<List<Barbeiro>> IBarbeiroRepository.GetByEspecialidade(string especialidade)
    {
        var barbeiros = await _context.Barbeiros.Where(b => b.Especialidade == especialidade).ToListAsync();
        return barbeiros;
    }

    async Task<Barbeiro> IRepositoryBase<Barbeiro>.GetById(int id)
    {
        var barbeiro = await _context.Barbeiros.FirstOrDefaultAsync(b => b.Id == id);
        return barbeiro;
    }

    Task IBarbeiroRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Barbeiro> IRepositoryBase<Barbeiro>.Update(Barbeiro entity)
    {
        _context.Barbeiros.Update(entity);
        return Task.FromResult(entity);
    }
}
