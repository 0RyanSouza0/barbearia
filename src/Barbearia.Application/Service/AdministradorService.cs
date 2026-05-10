using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Administrador;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using FluentValidation;

namespace Barbearia.Application.Service;

public class AdministradorService(IAdministradorRepository _repository, IValidator<AdministradorRequest> validator) : IAdministradorService
{
    async Task<Result<AdministradorResponse>> IAdministradorService.CreateAsync(AdministradorRequest request)
    {
        var resultValidator = await validator.ValidateAsync(request);
        if (!resultValidator.IsValid)
        {
            return Result<AdministradorResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var email = await _repository.GetByEmail(request.Email);
        if (email != null)
        {
            return Result<AdministradorResponse>.Failure("Email já cadastrado");
        }
        var senha = BCrypt.Net.BCrypt.HashPassword(request.Senha);
        var administrador = new Administrador
        (
            request.Nome,
            request.Email,
            senha
        );
        administrador.CriadoEm = DateTime.UtcNow;
        await _repository.Add(administrador);
        await _repository.SaveChangesAsync();
        return Result<AdministradorResponse>.Success(new AdministradorResponse
        {
            Id = administrador.Id,
            Nome = administrador.Nome,
            Email = administrador.Email,
            Ativo = administrador.Ativo,
            Role = administrador.Role,
            CriadoEm = administrador.CriadoEm
        });
    }

    async Task<Result<string>> IAdministradorService.DeleteAsync(int id)
    {
        var admin = await _repository.GetById(id);
        if (admin is null)
        {
            return Result<string>.Failure("Administrador não encontrado");
        }
        admin.Ativo = false;
        admin.AtualizadoEm = DateTime.UtcNow;
        await _repository.Update(admin);
        await _repository.SaveChangesAsync();
        return Result<string>.Success("Administrador deletado com sucesso");
    }

    async Task<Result<IEnumerable<AdministradorResponse>>> IAdministradorService.GetAllAsync()
    {
        var admins = await _repository.GetAll();
        return Result<IEnumerable<AdministradorResponse>>.Success(admins.Select(a => new AdministradorResponse
        {
            Id = a.Id,
            Nome = a.Nome,
            Email = a.Email,
            Role = a.Role,
            CriadoEm = a.CriadoEm,
            AtualizadoEm = a.AtualizadoEm.Value,
            Ativo = a.Ativo
        }));
    }

    async Task<Result<IEnumerable<AdministradorResponse>>> IAdministradorService.GetAllRelatorioAsync()
    {
        var admins = await _repository.GetAllAdministradoresRelatorio();
        return Result<IEnumerable<AdministradorResponse>>.Success(admins.Select(a => new AdministradorResponse
        {
            Id = a.Id,
            Nome = a.Nome,
            Email = a.Email,
            Role = a.Role,
            CriadoEm = a.CriadoEm,
            AtualizadoEm = a.AtualizadoEm.Value,
            Ativo = a.Ativo
        }));
    }

    async Task<Result<AdministradorResponse>> IAdministradorService.GetByEmailAsync(string email)
    {
        var admin = await _repository.GetByEmail(email);
        if (admin is null)
        {
            return Result<AdministradorResponse>.Failure("Administrador não encontrado");
        }
        return Result<AdministradorResponse>.Success(new AdministradorResponse
        {
            Id = admin.Id,
            Nome = admin.Nome,
            Email = admin.Email,
            Role = admin.Role,
            CriadoEm = admin.CriadoEm,
            AtualizadoEm = admin.AtualizadoEm.Value,
            Ativo = admin.Ativo
        });
    }

    async Task<Result<AdministradorResponse>> IAdministradorService.GetByIdAsync(int id)
    {
        var admin = await _repository.GetById(id);
        if (admin is null)
        {
            return Result<AdministradorResponse>.Failure("Administrador não encontrado");
        }
        return Result<AdministradorResponse>.Success(new AdministradorResponse
        {
            Id = admin.Id,
            Nome = admin.Nome,
            Email = admin.Email,
            Role = admin.Role,
            CriadoEm = admin.CriadoEm,
            AtualizadoEm = admin.AtualizadoEm.Value,
            Ativo = admin.Ativo
        });
    }

    async Task<Result<AdministradorResponse>> IAdministradorService.UpdateAsync(int id, AdministradorRequest request)
    {
        var admin = await _repository.GetById(id);
        if (admin is null)
        {
            return Result<AdministradorResponse>.Failure("Administrador não encontrado");
        }
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var email = await _repository.GetByEmail(request.Email);
            if (email != null && email.Id != id)
            {
                return Result<AdministradorResponse>.Failure("Email já cadastrado");
            }
            admin.Email = request.Email;
        }
        if (!string.IsNullOrWhiteSpace(request.Nome))
        {
            admin.Nome = request.Nome;
        }
        if (!string.IsNullOrWhiteSpace(request.Senha))
        {
            var senha = BCrypt.Net.BCrypt.HashPassword(request.Senha);
            admin.Senha = senha;
        }
        admin.AtualizadoEm = DateTime.UtcNow;
        await _repository.Update(admin);
        await _repository.SaveChangesAsync();
        return Result<AdministradorResponse>.Success(new AdministradorResponse
        {
            Id = admin.Id,
            Nome = admin.Nome,
            Email = admin.Email,
            Role = admin.Role,
            CriadoEm = admin.CriadoEm,
            AtualizadoEm = admin.AtualizadoEm.Value,
            Ativo = admin.Ativo
        });
    }
}

