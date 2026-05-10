using Barbearia.Application.Dtos.Administrador;
using FluentValidation;

namespace Barbearia.Application.Validators.Administrador;

public class AdministradorRequestValidator : AbstractValidator<AdministradorRequest>
{
    public AdministradorRequestValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Email deve ser válido")
            .NotEmpty().WithMessage("Email deve ser preenchido");
        RuleFor(c => c.Senha)
            .NotEmpty().WithMessage("Senha deve ser preenchida")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres");
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido");
    }
}
