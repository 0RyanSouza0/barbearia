using Barbearia.Application.Interfaces.Repository;
using Barbearia.Domain.Entities;
using Barbearia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Infra.Repository;

public class AgendamentoRepository(AppDbContext _context) : IAgendamentoRepository
{
    async Task<Agendamento> IRepositoryBase<Agendamento>.Add(Agendamento entity)
    {
        var agendamento = await _context.Agendamentos.AddAsync(entity);
        return agendamento.Entity;
    }

    Task IRepositoryBase<Agendamento>.Delete(Agendamento entity)
    {
        _context.Agendamentos.Remove(entity);
        return Task.CompletedTask;
    }

    async Task<IEnumerable<Agendamento>> IRepositoryBase<Agendamento>.GetAll()
    {
        var agendamentos = await _context.Agendamentos.ToListAsync();
        return agendamentos;
    }

    Task<Agendamento> IAgendamentoRepository.GetByClienteAsync(string nome)
    {
        var agendamento = _context.Agendamentos.FirstOrDefaultAsync(a => a.Cliente.Nome == nome);
        return agendamento;
    }

    Task<Agendamento> IRepositoryBase<Agendamento>.GetById(int id)
    {
        var agendamento = _context.Agendamentos.FirstOrDefaultAsync(a => a.Id == id);
        return agendamento;
    }

    Task IAgendamentoRepository.SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    Task<Agendamento> IRepositoryBase<Agendamento>.Update(Agendamento entity)
    {
        _context.Agendamentos.Update(entity);
        return Task.FromResult(entity);
    }
}
