using Barbearia.Application.Dtos.Produto;
using FluentValidation;

namespace Barbearia.Application.Validators.Produto;

public class ProdutoUpdateValidator : AbstractValidator<ProdutoUpdate>

{
    public ProdutoUpdateValidator()
    {
        RuleFor(p => p.Descricao)
            .NotEmpty().WithMessage("Descricao deve ser preenchido")
            .When(p => p.Descricao != null);
        RuleFor(p => p.Categoria)
            .NotEmpty().WithMessage("Categoria deve ser preenchido")
            .IsInEnum().WithMessage("Categoria inválida")
            .When(p => p.Categoria.HasValue);
        RuleFor(p => p.Valor)
            .NotEmpty().WithMessage("Valor deve ser preenchido")
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero")
            .When(p => p.Valor != null);
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("Nome deve ser preenchido")
            .When(p => p.Nome != null);
        RuleFor(p => p.Estoque)
            .NotEmpty().WithMessage("Estoque deve ser preenchido")
            .GreaterThan(0).WithMessage("Estoque deve ser maior que zero")
            .When(p => p.Estoque != null);
    }
}
