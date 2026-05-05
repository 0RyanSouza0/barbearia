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
            return Result<ClienteResponse>.Failure(result.Errors.First().ErrorMessage);
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

    Task<Result<string>> IClienteService.DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task<Result<IEnumerable<ClienteResponse>>> IClienteService.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<Result<ClienteResponse>> IClienteService.GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    Task<Result<ClienteResponse>> IClienteService.GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task<Result<ClienteResponse>> IClienteService.UpdateAsync(int id, ClienteUpdate update)
    {
        throw new NotImplementedException();
    }
}
