using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Servicos;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using FluentValidation;

namespace Barbearia.Application.Service;

public class ServicosService(IServicosRepository repository, IValidator<ServicosRequest> validatorRequest
, IValidator<ServicosUpdate> validatorUpdate) : IServicosService
{
    async Task<Result<ServicosResponse>> IServicosService.CreateAsync(ServicosRequest request)
    {
        var resultValidator = await validatorRequest.ValidateAsync(request);
        if (!resultValidator.IsValid)
        {
            return Result<ServicosResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var servico = new Servicos(request.Nome, request.Descricao, request.Valor, request.DuracaoMinutos);
        servico.CriadoEm = DateTime.UtcNow;
        await repository.Add(servico);
        await repository.SaveChangesAsync();
        return Result<ServicosResponse>.Success(new ServicosResponse
        {
            Id = servico.Id,
            Nome = servico.Nome,
            Descricao = servico.Descricao,
            DuracaoMinutos = servico.DuracaoMinutos,
            Valor = servico.Valor
        });
    }

    async Task<Result<string>> IServicosService.DeleteAsync(int id)
    {
        var servico = await repository.GetById(id)
;
        if (servico is null)
        {
            return Result<string>.Failure("Servico não encontrado");
        }
        await repository.Delete(servico);
        await repository.SaveChangesAsync();
        return Result<string>.Success("Servico deletado do sistema");
    }

    async Task<Result<IEnumerable<ServicosResponse>>> IServicosService.GetAllAsync()
    {
        var servicos = await repository.GetAll();
        return Result<IEnumerable<ServicosResponse>>.Success(servicos.Select(s => new ServicosResponse
        {
            Id = s.Id,
            Nome = s.Nome,
            Descricao = s.Descricao,
            DuracaoMinutos = s.DuracaoMinutos,
            Valor = s.Valor
        }));
    }

    async Task<Result<ServicosResponse>> IServicosService.GetByIdAsync(int id)
    {
        var servico = await repository.GetById(id);
        return Result<ServicosResponse>.Success(new ServicosResponse
        {
            Id = servico.Id,
            Nome = servico.Nome,
            Descricao = servico.Descricao,
            DuracaoMinutos = servico.DuracaoMinutos,
            Valor = servico.Valor
        });
    }

    async Task<Result<ServicosResponse>> IServicosService.GetByIdNome(string nome)
    {
        var servico = await repository.GetByName(nome);
        if (servico is null)
        {
            return Result<ServicosResponse>.Failure("Servico não encontrado");
        }
        return Result<ServicosResponse>.Success(new ServicosResponse
        {
            Id = servico.Id,
            Nome = servico.Nome,
            Descricao = servico.Descricao,
            DuracaoMinutos = servico.DuracaoMinutos,
            Valor = servico.Valor
        });
    }

    async Task<Result<ServicosResponse>> IServicosService.UpdateAsync(int id, ServicosUpdate update)
    {
        var resultValidator = await validatorUpdate.ValidateAsync(update);
        if (!resultValidator.IsValid)
        {
            return Result<ServicosResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var servico = await repository.GetById(id);
        if (servico is null)
        {
            return Result<ServicosResponse>.Failure("Servico não encontrado");
        }
        if (!string.IsNullOrWhiteSpace(update.Nome))
        {
            servico.Nome = update.Nome;
        }
        if (!string.IsNullOrWhiteSpace(update.Descricao))
        {
            servico.Descricao = update.Descricao;
        }
        if (update.Valor.HasValue)
        {
            servico.Valor = update.Valor.Value;
        }
        if (update.DuracaoMinutos.HasValue)
        {
            servico.DuracaoMinutos = update.DuracaoMinutos.Value;
        }
        servico.AtualizadoEm = DateTime.UtcNow;
        await repository.Update(servico);
        await repository.SaveChangesAsync();
        return Result<ServicosResponse>.Success(new ServicosResponse
        {
            Id = servico.Id,
            Nome = servico.Nome,
            Descricao = servico.Descricao,
            DuracaoMinutos = servico.DuracaoMinutos,
            Valor = servico.Valor
        });
    }
}
