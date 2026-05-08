using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Agendamento;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using Barbearia.Domain.Enuns;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Barbearia.Application.Service;

public class AgendamentoService(IAgendamentoRepository repository, IValidator<AgendamentoRequest> _validatorRequest, IValidator<AgendamentoUpdate> _validatorUpdate) : IAgendamentoService
{
    async Task<Result<AgendamentoResponse>> IAgendamentoService.CreateAysnc(AgendamentoRequest request)
    {
        var resultValidator = await _validatorRequest.ValidateAsync(request);
        if (!resultValidator.IsValid)
        {
            var erros = resultValidator.Errors.Select(e => e.ErrorMessage).ToList();
            return Result<AgendamentoResponse>.Failure(erros);
        }
        var agendamento = new Agendamento(
            request.ClienteId,
            request.ServicoId,
            request.BarbeiroId,
            request.DataHora
        );
        agendamento.CriadoEm = DateTime.UtcNow;
        await repository.Add(agendamento);
        await repository.SaveChangesAsync();
        return Result<AgendamentoResponse>.Success(new AgendamentoResponse
        {
            Id = agendamento.Id,
            BarbeiroId = agendamento.BarbeiroId,
            ClienteId = agendamento.ClienteId,
            StatusAgendamento = agendamento.Status,
            DataHora = agendamento.DataHora
        });
    }

    async Task<Result<string>> IAgendamentoService.DeleteAsync(int id)
    {
        var agendamento = await repository.GetById(id);
        if (agendamento is null)
        {
            return Result<string>.Failure("Agendamento não encontrado");
        }
        if (agendamento.Status == StatusAgendamento.Concluido)
        {
            return Result<string>.Failure("Agendamento concluido nao pode ser deletado");
        }
        await repository.Delete(agendamento);
        await repository.SaveChangesAsync();
        return Result<string>.Success("Agendamento deletado com sucesso");
    }

    async Task<Result<IEnumerable<AgendamentoResponse>>> IAgendamentoService.GetAllAsync()
    {
        var agendamentos = await repository.GetAll();
        return Result<IEnumerable<AgendamentoResponse>>.Success(agendamentos.Select(a => new AgendamentoResponse
        {
            Id = a.Id,
            BarbeiroId = a.BarbeiroId,
            ClienteId = a.ClienteId,
            StatusAgendamento = a.Status,
            DataHora = a.DataHora
        }));
    }

    async Task<Result<AgendamentoResponse>> IAgendamentoService.GetByClienteAsync(string nome)
    {
        var agendamento = await repository.GetByClienteAsync(nome);
        if (agendamento is null)
        {
            return Result<AgendamentoResponse>.Failure("Agendamento não encontrado");
        }
        return Result<AgendamentoResponse>.Success(new AgendamentoResponse
        {
            Id = agendamento.Id,
            BarbeiroId = agendamento.BarbeiroId,
            ClienteId = agendamento.ClienteId,
            StatusAgendamento = agendamento.Status,
            DataHora = agendamento.DataHora
        });
    }

    async Task<Result<AgendamentoResponse>> IAgendamentoService.GetByIdAsync(int id)
    {
        var agendamento = await repository.GetById(id);
        if (agendamento is null)
        {
            return Result<AgendamentoResponse>.Failure("Agendamento não encontrado");
        }
        return Result<AgendamentoResponse>.Success(new AgendamentoResponse
        {
            Id = agendamento.Id,
            BarbeiroId = agendamento.BarbeiroId,
            ClienteId = agendamento.ClienteId,
            StatusAgendamento = agendamento.Status,
            DataHora = agendamento.DataHora
        });
    }

    async Task<Result<AgendamentoResponse>> IAgendamentoService.UpdateAsync(int id, AgendamentoUpdate update)
    {
        var resultValidator = await _validatorUpdate.ValidateAsync(update);
        if (!resultValidator.IsValid)
        {
            return Result<AgendamentoResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var agendamento = await repository.GetById(id);
        if (agendamento is null)
        {
            return Result<AgendamentoResponse>.Failure("Agendamento não encontrado");
        }
        try
        {
            agendamento.ParaAtualizarAgendamento();
            if (update.DataHora.HasValue)
            {
                agendamento.DataHora = update.DataHora.Value;
            }
            agendamento.AtualizadoEm = DateTime.UtcNow;
            await repository.Update(agendamento);
            await repository.SaveChangesAsync();
            return Result<AgendamentoResponse>.Success(new AgendamentoResponse
            {
                Id = agendamento.Id,
                BarbeiroId = agendamento.BarbeiroId,
                ClienteId = agendamento.ClienteId,
                StatusAgendamento = agendamento.Status,
                DataHora = agendamento.DataHora
            });
        }
        catch (InvalidOperationException ex)
        {
            return Result<AgendamentoResponse>.Failure(ex.Message);
        }

    }
}
