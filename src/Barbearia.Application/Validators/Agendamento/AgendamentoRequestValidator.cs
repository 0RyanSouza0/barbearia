using Barbearia.Application.Dtos.Agendamento;
using FluentValidation;

namespace Barbearia.Application.Validators.Agendamento;

public class AgendamentoRequestValidator : AbstractValidator<AgendamentoRequest>
{
    public AgendamentoRequestValidator()
    {
        RuleFor(a => a.ClienteId)
            .NotEmpty().WithMessage("ClienteId deve ser preenchido");
        RuleFor(a => a.ServicoId)
            .NotEmpty().WithMessage("ServicoId deve ser preenchido");
        RuleFor(a => a.BarbeiroId)
            .NotEmpty().WithMessage("BarbeiroId deve ser preenchido");
        RuleFor(a => a.DataHora)
            .NotEmpty().WithMessage("DataHora deve ser preenchido");
    }
}
