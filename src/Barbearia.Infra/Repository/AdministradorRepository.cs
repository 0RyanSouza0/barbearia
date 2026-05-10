using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class AdministradorRepository(AppDbContext _context) : IAdministradorRepository
{
    async Task<Administrador> IRepositoryBase<Administrador>.Add(Administrador entity)
    {
        var admin = await _context.Administradors.AddAsync(entity);
        return admin.Entity;
    }

    Task IRepositoryBase<Administrador>.Delete(Administrador entity)
    {
        throw new NotImplementedException();
    }

    async Task<IEnumerable<Administrador>> IRepositoryBase<Administrador>.GetAll()
    {
        var admins = await _context.Administradors.AsNoTracking().ToListAsync();
        return admins;
    }

    async Task<IEnumerable<Administrador>> IAdministradorRepository.GetAllAdministradoresRelatorio()
    {
        var admins = await _context.Administradors.IgnoreQueryFilters().AsNoTracking().ToListAsync();
        return admins;

    }

    async Task<Administrador> IAdministradorRepository.GetByEmail(string email)
    {
        var admin = await _context.Administradors.FirstOrDefaultAsync(a => a.Email == email);
        return admin;
    }

    async Task<Administrador> IRepositoryBase<Administrador>.GetById(int id)
    {
        var admin = await _context.Administradors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        return admin;
    }

    Task IAdministradorRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Administrador> IRepositoryBase<Administrador>.Update(Administrador entity)
    {
        var admin = _context.Administradors.Update(entity);
        return Task.FromResult(admin.Entity);
    }
}
