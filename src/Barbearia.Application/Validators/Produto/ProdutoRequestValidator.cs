using System.Data;
using Barbearia.Application.Dtos.Produto;
using FluentValidation;

namespace Barbearia.Application.Validators.Produto;

public class ProdutoRequestValidator : AbstractValidator<ProdutoRequest>
{
    public ProdutoRequestValidator()
    {
        RuleFor(p => p.Descricao)
            .NotEmpty().WithMessage("Descricao deve ser preenchido");
        RuleFor(p => p.CategoriaId)
            .NotEmpty().WithMessage("CategoriaId deve ser preenchido");
        RuleFor(p => p.Valor)
            .NotEmpty().WithMessage("Valor deve ser preenchido")
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero");
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido");
        RuleFor(p => p.Estoque)
            .NotEmpty().WithMessage("Estoque deve ser preenchido")
            .GreaterThan(0).WithMessage("Estoque deve ser maior que zero");
    }
}
