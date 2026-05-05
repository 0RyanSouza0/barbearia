using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class EnderecoRepository(AppDbContext _context) : IEnderecoRepository
{
    async Task<Endereco> IRepositoryBase<Endereco>.Add(Endereco entity)
    {
        var endereco = await _context.Enderecos.AddAsync(entity);
        return endereco.Entity;
    }

    Task IRepositoryBase<Endereco>.Delete(Endereco entity)
    {
        _context.Enderecos.Remove(entity);
        return Task.CompletedTask;
    }

    async Task<IEnumerable<Endereco>> IRepositoryBase<Endereco>.GetAll()
    {
        var enderecos = await _context.Enderecos.ToListAsync();
        return enderecos;
    }

    async Task<Endereco?> IRepositoryBase<Endereco>.GetById(int id)
    {
        var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == id);
        return endereco;
    }

    Task IEnderecoRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Endereco> IRepositoryBase<Endereco>.Update(Endereco entity)
    {
        _context.Enderecos.Update(entity);
        return Task.FromResult(entity);
    }
}
