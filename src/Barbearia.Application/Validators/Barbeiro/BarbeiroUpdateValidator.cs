using Barbearia.Application.Dtos.Barbeiro;
using FluentValidation;

namespace Barbearia.Application.Validators.Barbeiro;

public class BarbeiroUpdateValidator : AbstractValidator<BarbeiroUpdate>
{
    public BarbeiroUpdateValidator()
    {
        RuleFor(b => b.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido")
            .When(b => b.Nome != null);
        RuleFor(b => b.Especialidade)
            .NotEmpty().WithMessage("Especialidade deve ser preenchido")
            .When(b => b.Especialidade != null);

    }
}
