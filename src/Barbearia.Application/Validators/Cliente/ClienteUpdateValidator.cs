using Barbearia.Application.Dtos.Cliente;
using FluentValidation;

namespace Barbearia.Application.Validators.Cliente;

public class ClienteUpdateValidator : AbstractValidator<ClienteUpdate>
{
    public ClienteUpdateValidator()
    {
        RuleFor(c => c.Email)
           .EmailAddress().WithMessage("Email deve ser válido")
           .NotEmpty().WithMessage("Email deve ser preenchido")
           .When(c => c.Email != null);
        RuleFor(c => c.Senha)
            .NotEmpty().WithMessage("Senha deve ser preenchida")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres")
            .When(c => c.Senha != null);
        RuleFor(c => c.Telefone)
            .NotEmpty().WithMessage("Telefone deve ser preenchido")
            .MinimumLength(11).WithMessage("Telefone deve ter pelo menos 11 caracteres")
            .When(c => c.Telefone != null);
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido")
            .When(c => c.Nome != null);
    }
}
