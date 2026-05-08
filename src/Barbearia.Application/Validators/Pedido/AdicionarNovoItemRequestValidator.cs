using System.Data;
using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class AdicionarNovoItemRequestValidator : AbstractValidator<AdicionarItemRequest>
{
    public AdicionarNovoItemRequestValidator()
    {
        RuleFor(q => q.ProdutoId)
            .NotEmpty().WithMessage("IdProduto deve ser preenchido");
        RuleFor(q => q.Quantidade)
            .NotEmpty().WithMessage("Quantidade deve ser preenchido")
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero");
    }
}
