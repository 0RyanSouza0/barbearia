using Barbearia.Application.Dtos.Barbeiro;
using FluentValidation;

namespace Barbearia.Application.Validators.Barbeiro;

public class BarbeiroRequestValidator : AbstractValidator<BarbeiroRequest>
{
    public BarbeiroRequestValidator()
    {
        RuleFor(b => b.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido");
        RuleFor(b => b.Especialidade)
            .NotEmpty().WithMessage("Especialidade deve ser preenchido");
    }
}
