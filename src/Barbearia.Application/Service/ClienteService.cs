using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Cliente;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Application.Validators.Cliente;
using Barbearia.Domain.Entities;
using FluentValidation;

namespace Barbearia.Application.Service;

public class ClienteService(IClienteRepository _repository, IValidator<ClienteRequest> _validatorRequest,
 IValidator<ClienteUpdate> _validatorUpdate) : IClienteService
{
    async Task<Result<ClienteResponse>> IClienteService.CreateAsync(ClienteRequest request)
    {
        var result = await _validatorRequest.ValidateAsync(request);
        if (!result.IsValid)
        {
            return Result<ClienteResponse>.Failure(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var email = await _repository.GetByEmailAsync(request.Email);
        if (email != null)
        {
            return Result<ClienteResponse>.Failure("Email já cadastrado");
        }
        var senha = BCrypt.Net.BCrypt.HashPassword(request.Senha);
        var cliente = new Cliente
        (
            request.Nome,
            request.Telefone,
            request.Email,
            senha
        );
        cliente.CriadoEm = DateTime.UtcNow;
        await _repository.Add(cliente);
        await _repository.SaveChangesAsync();


        return Result<ClienteResponse>.Success(new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Email = cliente.Email
        });

    }

    async Task<Result<string>> IClienteService.DeleteAsync(int id)
    {
        var cliente = await _repository.GetById(id);
        if (cliente is null)
        {
            return Result<string>.Failure("Cliente não encontrado");
        }
        cliente.Ativo = false;
        return Result<string>.Success("Cliente deletado com sucesso");
    }

    async Task<Result<IEnumerable<ClienteResponse>>> IClienteService.GetAllAsync()
    {
        var clientes = await _repository.GetAll();
        return Result<IEnumerable<ClienteResponse>>.Success(clientes.Select(c => new ClienteResponse
        {
            Id = c.Id,
            Nome = c.Nome,
            Telefone = c.Telefone,
            Email = c.Email
        }));
    }

    async Task<Result<ClienteResponse>> IClienteService.GetByEmailAsync(string email)
    {
        var cliente = await _repository.GetByEmailAsync(email);
        if (cliente is null)
        {
            return Result<ClienteResponse>.Failure("Cliente não encontrado");
        }
        return Result<ClienteResponse>.Success(new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Email = cliente.Email
        });
    }

    async Task<Result<ClienteResponse>> IClienteService.GetByIdAsync(int id)
    {
        var cliente = await _repository.GetById(id);
        if (cliente is null)
        {
            return Result<ClienteResponse>.Failure("Cliente não encontrado");
        }
        return Result<ClienteResponse>.Success(new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Email = cliente.Email
        });
    }

    async Task<Result<ClienteResponse>> IClienteService.UpdateAsync(int id, ClienteUpdate update)
    {
        var cliente = await _repository.GetById(id);
        if (cliente is null)
        {
            return Result<ClienteResponse>.Failure("Cliente não encontrado");
        }
        var resultValidator = await _validatorUpdate.ValidateAsync(update);
        if (!resultValidator.IsValid)
        {
            return Result<ClienteResponse>.Failure(resultValidator.Errors.First().ErrorMessage);
        }
        if (!string.IsNullOrWhiteSpace(update.Email))
        {
            var email = await _repository.GetByEmailAsync(update.Email);
            if (email != null && email.Id != id)
            {
                return Result<ClienteResponse>.Failure("Email já cadastrado");
            }
            cliente.Email = update.Email;
        }

        if (!string.IsNullOrWhiteSpace(update.Senha))
        {
            var senha = BCrypt.Net.BCrypt.HashPassword(update.Senha);
            cliente.Senha = senha;
        }
        if (!string.IsNullOrWhiteSpace(update.Telefone))
        {
            cliente.Telefone = update.Telefone;
        }
        if (!string.IsNullOrWhiteSpace(update.Nome))
        {
            cliente.Nome = update.Nome;
        }

        cliente.AtualizadoEm = DateTime.UtcNow;
        await _repository.Update(cliente);
        await _repository.SaveChangesAsync();

        return Result<ClienteResponse>.Success(new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Email = cliente.Email
        });
    }
}
