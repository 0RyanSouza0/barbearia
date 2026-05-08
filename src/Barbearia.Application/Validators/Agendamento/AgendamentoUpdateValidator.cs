using Barbearia.Application.Dtos.Agendamento;
using FluentValidation;

namespace Barbearia.Application.Validators.Agendamento;

public class AgendamentoUpdateValidator : AbstractValidator<AgendamentoUpdate>
{
    public AgendamentoUpdateValidator()
    {
        RuleFor(a => a.DataHora)
            .NotEmpty().WithMessage("DataHora deve ser preenchido")
            .GreaterThan(DateTime.Now).WithMessage("DataHora deve ser maior que a data atual")
            .When(a => a.DataHora != null);

    }
}
