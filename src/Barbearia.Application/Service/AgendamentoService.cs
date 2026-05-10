using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Agendamento;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using Barbearia.Domain.Enuns;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Barbearia.Application.Service;

public class AgendamentoService(IAgendamentoRepository repository, IServicosRepository _servicosRepository,
IBarbeiroRepository _barbeiroRepository, IClienteRepository _clienteRepository, IValidator<AgendamentoRequest> _validatorRequest, IValidator<AgendamentoUpdate> _validatorUpdate) : IAgendamentoService
{
    async Task<Result<AgendamentoResponse>> IAgendamentoService.CreateAysnc(AgendamentoRequest request)
    {
        var resultValidator = await _validatorRequest.ValidateAsync(request);
        if (!resultValidator.IsValid)
        {
            var erros = resultValidator.Errors.Select(e => e.ErrorMessage).ToList();
            return Result<AgendamentoResponse>.Failure(erros);
        }
        var cliente = await _clienteRepository.GetById(request.ClienteId);
        if (cliente is null)
        {
            return Result<AgendamentoResponse>.Failure("Cliente não encontrado");
        }
        var servico = await _servicosRepository.GetById(request.ServicosId);
        if (servico is null)
        {
            return Result<AgendamentoResponse>.Failure("Serviço não encontrado");
        }
        var barbeiro = await _barbeiroRepository.GetById(request.BarbeiroId);
        if (barbeiro is null)
        {
            return Result<AgendamentoResponse>.Failure("Barbeiro não encontrado");
        }
        var agendamento = new Agendamento(
            request.ClienteId,
            request.ServicosId,
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
            BarbeiroNome = barbeiro.Nome,
            ClienteId = agendamento.ClienteId,
            NomeCliente = cliente.Nome,
            ServicosId = agendamento.ServicosId,
            NomeServico = servico.Nome,
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
            BarbeiroNome = a.Barbeiro.Nome,
            ClienteId = a.ClienteId,
            NomeCliente = a.Cliente.Nome,
            ServicosId = a.ServicosId,
            NomeServico = a.Servicos.Nome,
            StatusAgendamento = a.Status,
            DataHora = a.DataHora
        }));
    }

    async Task<Result<IList<AgendamentoResponse>>> IAgendamentoService.GetByClienteAsync(string nome)
    {
        var agendamentos = await repository.GetByClienteAsync(nome);
        if (agendamentos is null)
        {
            return Result<IList<AgendamentoResponse>>.Failure("Agendamento não encontrado");
        }
        return Result<IList<AgendamentoResponse>>.Success(agendamentos.Select(a => new AgendamentoResponse
        {
            Id = a.Id,
            BarbeiroId = a.BarbeiroId,
            BarbeiroNome = a.Barbeiro.Nome,
            ClienteId = a.ClienteId,
            NomeCliente = a.Cliente.Nome,
            ServicosId = a.ServicosId,
            NomeServico = a.Servicos.Nome,
            StatusAgendamento = a.Status,
            DataHora = a.DataHora
        }).ToList());
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
            BarbeiroNome = agendamento.Barbeiro.Nome,
            ClienteId = agendamento.ClienteId,
            NomeCliente = agendamento.Cliente.Nome,
            ServicosId = agendamento.ServicosId,
            NomeServico = agendamento.Servicos.Nome,
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
                BarbeiroNome = agendamento.Barbeiro.Nome,
                ClienteId = agendamento.ClienteId,
                NomeCliente = agendamento.Cliente.Nome,
                ServicosId = agendamento.ServicosId,
                NomeServico = agendamento.Servicos.Nome,
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
