using System.Data;
using Barbearia.Application.Dtos.Servicos;
using FluentValidation;

namespace Barbearia.Application.Validators.Servicos;

public class ServicosRequestValidator : AbstractValidator<ServicosRequest>
{
    public ServicosRequestValidator()
    {
        RuleFor(s => s.Descricao)
            .NotEmpty().WithMessage("Descricao deve ser preenchido");
        RuleFor(s => s.Valor)
            .NotEmpty().WithMessage("Valor deve ser preenchido")
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero");
        RuleFor(s => s.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido");
        RuleFor(s => s.DuracaoMinutos)
            .NotEmpty().WithMessage("DuracaoMinutos deve ser preenchido")
            .GreaterThan(0).WithMessage("DuracaoMinutos deve ser maior que zero");
    }
}
