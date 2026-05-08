using Barbearia.Application.Dtos.Servicos;
using FluentValidation;

namespace Barbearia.Application.Validators.Servicos;

public class ServicosUpdateValidator : AbstractValidator<ServicosUpdate>
{
    public ServicosUpdateValidator()
    {
        RuleFor(s => s.Descricao)
            .NotEmpty().WithMessage("Descricao deve ser preenchido")
            .When(s => s.Descricao != null);
        RuleFor(s => s.Valor)
            .NotEmpty().WithMessage("Valor deve ser preenchido")
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero")
            .When(s => s.Valor.HasValue);
        RuleFor(s => s.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido")
            .When(s => s.Nome != null);
        RuleFor(s => s.DuracaoMinutos)
            .NotEmpty().WithMessage("DuracaoMinutos deve ser preenchido")
            .GreaterThan(0).WithMessage("DuracaoMinutos deve ser maior que zero")
            .When(s => s.DuracaoMinutos.HasValue);

    }

}
