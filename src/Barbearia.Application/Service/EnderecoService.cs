using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Endereco;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using FluentValidation;

namespace Barbearia.Application.Service;

public class EnderecoService(IEnderecoRepository _repository, IValidator<EnderecoRequest> _validatorRequest, IValidator<EnderecoUpdate> _validatorUpdate) : IEnderecoService
{
    async Task<Result<EnderecoResponse>> IEnderecoService.CreateAsync(EnderecoRequest enderecoRequest)
    {
        var resultValidator = await _validatorRequest.ValidateAsync(enderecoRequest);
        if (!resultValidator.IsValid)
        {
            return Result<EnderecoResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var endereco = new Endereco(
            enderecoRequest.Logradouro,
            enderecoRequest.Cep,
            enderecoRequest.Bairro,
            enderecoRequest.Numero,
            enderecoRequest.Cidade,
            enderecoRequest.Estado
        );
        endereco.CriadoEm = DateTime.UtcNow;
        await _repository.Add(endereco);
        await _repository.SaveChangesAsync();
        return Result<EnderecoResponse>.Success(new EnderecoResponse
        {
            Bairro = endereco.Bairro,
            Cep = endereco.Cep,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            CriadoEm = endereco.CriadoEm,
            Id = endereco.Id,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero
        });
    }

    async Task<Result<string>> IEnderecoService.DeleteAsync(int id)
    {
        var endereco = await _repository.GetById(id);
        if (endereco is null)
        {
            return Result<string>.Failure("Endereco não encontrado");
        }
        await _repository.Delete(endereco);
        await _repository.SaveChangesAsync();
        return Result<string>.Success("Endereco deletado do sistema");
    }

    async Task<Result<IEnumerable<EnderecoResponse>>> IEnderecoService.GetAllAsync()
    {
        var enderecos = await _repository.GetAll();
        return Result<IEnumerable<EnderecoResponse>>.Success(enderecos.Select(e => new EnderecoResponse
        {
            Bairro = e.Bairro,
            Cep = e.Cep,
            Cidade = e.Cidade,
            Estado = e.Estado,
            CriadoEm = e.CriadoEm,
            AtualizadoEm = e.AtualizadoEm.Value,
            Id = e.Id,
            Logradouro = e.Logradouro,
            Numero = e.Numero
        }));
    }

    async Task<Result<EnderecoResponse>> IEnderecoService.GetByIdAsync(int id)
    {
        var endereco = await _repository.GetById(id);
        if (endereco is null)
        {
            return Result<EnderecoResponse>.Failure("Endereco não encontrado");
        }
        return Result<EnderecoResponse>.Success(new EnderecoResponse
        {
            Bairro = endereco.Bairro,
            Cep = endereco.Cep,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            CriadoEm = endereco.CriadoEm,
            AtualizadoEm = endereco.AtualizadoEm.Value,
            Id = endereco.Id,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero
        });
    }

    async Task<Result<EnderecoResponse>> IEnderecoService.UpdateAsync(int id, EnderecoUpdate update)
    {
        var resultValidator = await _validatorUpdate.ValidateAsync(update);
        if (!resultValidator.IsValid)
        {
            return Result<EnderecoResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var endereco = await _repository.GetById(id);
        if (endereco is null)
        {
            return Result<EnderecoResponse>.Failure("Endereco não encontrado");
        }
        if (!string.IsNullOrWhiteSpace(update.Bairro))
        {
            endereco.Bairro = update.Bairro;
        }
        if (!string.IsNullOrWhiteSpace(update.Cep))
        {
            endereco.Cep = update.Cep;
        }
        if (!string.IsNullOrWhiteSpace(update.Cidade))
        {
            endereco.Cidade = update.Cidade;
        }
        if (!string.IsNullOrWhiteSpace(update.Estado))
        {
            endereco.Estado = update.Estado;
        }
        if (!string.IsNullOrWhiteSpace(update.Logradouro))
        {
            endereco.Logradouro = update.Logradouro;
        }
        if (update.Numero.HasValue)
        {
            endereco.Numero = update.Numero.Value;
        }
        endereco.AtualizadoEm = DateTime.UtcNow;
        await _repository.Update(endereco);
        await _repository.SaveChangesAsync();
        return Result<EnderecoResponse>.Success(new EnderecoResponse
        {
            Bairro = endereco.Bairro,
            Cep = endereco.Cep,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            CriadoEm = endereco.CriadoEm,
            AtualizadoEm = endereco.AtualizadoEm.Value,
            Id = endereco.Id,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero
        });
    }
}
