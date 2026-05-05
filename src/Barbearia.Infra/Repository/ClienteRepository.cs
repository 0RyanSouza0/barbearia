using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class ClienteRepository(AppDbContext _context) : IClienteRepository
{

    async Task<Cliente> IRepositoryBase<Cliente>.Add(Cliente entity)
    {
        var cliente = await _context.Clientes.AddAsync(entity);
        return cliente.Entity;
    }

    Task IRepositoryBase<Cliente>.Delete(Cliente entity)
    {
        throw new NotImplementedException();
    }

    async Task<IEnumerable<Cliente>> IRepositoryBase<Cliente>.GetAll()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return clientes;
    }

    async Task<Cliente?> IClienteRepository.GetByEmailAsync(string email)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
        return cliente;
    }

    async Task<Cliente?> IRepositoryBase<Cliente>.GetById(int id)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        return cliente;
    }

    Task<Cliente> IRepositoryBase<Cliente>.Update(Cliente entity)
    {
        var cliente = _context.Clientes.Update(entity);
        return Task.FromResult(entity);
    }
}
