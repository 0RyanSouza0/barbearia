using Barbearia.Application.Dtos.Agendamento;
using FluentValidation;

namespace Barbearia.Application.Validators.Agendamento;

public class AgendamentoUpdateValidator : AbstractValidator<AgendamentoUpdate>
{
    public AgendamentoUpdateValidator()
    {
        RuleFor(a => a.ClienteId)
            .NotEmpty().WithMessage("ClienteId deve ser preenchido")
            .When(a => a.ClienteId != null);
        RuleFor(a => a.ServicoId)
            .NotEmpty().WithMessage("ServicoId deve ser preenchido")
            .When(a => a.ServicoId != null);
        RuleFor(a => a.BarbeiroId)
            .NotEmpty().WithMessage("BarbeiroId deve ser preenchido")
            .When(a => a.BarbeiroId != null);
        RuleFor(a => a.DataHora)
            .NotEmpty().WithMessage("DataHora deve ser preenchido")
            .When(a => a.DataHora != null);

    }
}
