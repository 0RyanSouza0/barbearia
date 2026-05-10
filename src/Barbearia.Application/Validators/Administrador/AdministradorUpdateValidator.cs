using Barbearia.Application.Dtos.Administrador;
using FluentValidation;

namespace Barbearia.Application.Validators.Administrador;

public class AdministradorUpdateValidator : AbstractValidator<AdministradorRequest>
{
    public AdministradorUpdateValidator()
    {

        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Email deve ser válido")
            .NotEmpty().WithMessage("Email deve ser preenchido")
            .When(c => c.Email != null);
        RuleFor(c => c.Senha)
            .NotEmpty().WithMessage("Senha deve ser preenchida")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres")
            .When(c => c.Senha != null);
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido")
            .When(c => c.Nome != null);
    }
}
