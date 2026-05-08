using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Barbeiro;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Barbearia.Application.Service
{
    public class BarbeiroService(IBarbeiroRepository _repository, IValidator<BarbeiroRequest> _validatorRequest,
    IValidator<BarbeiroUpdate> _validatorUpdate) : IBarbeiroService
    {
        async Task<Result<BarbeiroResponse>> IBarbeiroService.CreateAsync(BarbeiroRequest request)
        {
            var resultValidator = await _validatorRequest.ValidateAsync(request);
            if (!resultValidator.IsValid)
            {
                return Result<BarbeiroResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
            }
            var barbeiro = new Barbeiro(request.Nome, request.Especialidade);
            barbeiro.CriadoEm = DateTime.UtcNow;
            await _repository.Add(barbeiro);
            await _repository.SaveChangesAsync();

            return Result<BarbeiroResponse>.Success(new BarbeiroResponse
            {
                Id = barbeiro.Id,
                Nome = barbeiro.Nome,
                Especialidade = barbeiro.Especialidade
            });

        }

        async Task<Result<string>> IBarbeiroService.DeleteAsync(int id)
        {
            var barbeiro = await _repository.GetById(id);
            if (barbeiro is null)
            {
                return Result<string>.Failure("Barbeiro não encontrado");
            }
            await _repository.Delete(barbeiro);
            await _repository.SaveChangesAsync();
            return Result<string>.Success("Barbeiro deletado do sistema");

        }

        async Task<Result<IEnumerable<BarbeiroResponse>>> IBarbeiroService.GetAllAsync()
        {
            var barbeiros = await _repository.GetAll();
            return Result<IEnumerable<BarbeiroResponse>>.Success(barbeiros.Select(b => new BarbeiroResponse
            {
                Id = b.Id,
                Nome = b.Nome,
                Especialidade = b.Especialidade
            }));
        }

        async Task<Result<List<BarbeiroResponse>>> IBarbeiroService.GetByEspecialidadeAsync(string especialidade)
        {
            var barbeiro = await _repository.GetByEspecialidade(especialidade);
            if (barbeiro is null)
            {
                return Result<List<BarbeiroResponse>>.Failure("Barbeiro(s) não encontrado(s)");
            }
            return Result<List<BarbeiroResponse>>.Success(barbeiro.Select(b => new BarbeiroResponse
            {
                Id = b.Id,
                Nome = b.Nome,
                Especialidade = b.Especialidade
            }).ToList());
        }

        async Task<Result<BarbeiroResponse>> IBarbeiroService.GetByIdAsync(int id)
        {
            var barbeiro = await _repository.GetById(id);
            if (barbeiro is null)
            {
                return Result<BarbeiroResponse>.Failure("Barbeiro não encontrado");
            }
            return Result<BarbeiroResponse>.Success(new BarbeiroResponse
            {
                Id = barbeiro.Id,
                Nome = barbeiro.Nome,
                Especialidade = barbeiro.Especialidade
            });
        }

        async Task<Result<BarbeiroResponse>> IBarbeiroService.UpdateAsync(int id, BarbeiroUpdate update)
        {
            var barbeiro = await _repository.GetById(id);
            if (barbeiro is null)
            {
                return Result<BarbeiroResponse>.Failure("Barbeiro não encontrado");
            }
            var resultValidator = await _validatorUpdate.ValidateAsync(update);
            if (!resultValidator.IsValid)
            {
                return Result<BarbeiroResponse>.Failure(resultValidator.Errors.First().ErrorMessage);
            }
            if (!string.IsNullOrWhiteSpace(update.Nome))
            {
                barbeiro.Nome = update.Nome;
            }

            if (!string.IsNullOrWhiteSpace(update.Especialidade))
            {
                barbeiro.Especialidade = update.Especialidade;
            }
            barbeiro.AtualizadoEm = DateTime.UtcNow;
            await _repository.Update(barbeiro);
            await _repository.SaveChangesAsync();
            return Result<BarbeiroResponse>.Success(new BarbeiroResponse
            {
                Id = barbeiro.Id,
                Nome = barbeiro.Nome,
                Especialidade = barbeiro.Especialidade
            });
        }
    }
}