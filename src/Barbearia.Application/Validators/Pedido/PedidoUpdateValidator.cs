using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class PedidoUpdateValidator : AbstractValidator<PedidoUpdate>
{
    public PedidoUpdateValidator()
    {
        RuleForEach(p => p.Itens).ChildRules(item =>
{
    item.RuleFor(i => i.ProdutoId)
        .NotEmpty().WithMessage("ProdutoId deve ser preenchido")
        .When(i => i.ProdutoId.HasValue);


    item.RuleFor(i => i.Quantidade)
        .NotEmpty().WithMessage("Quantidade deve ser preenchido")
        .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero")
        .When(i => i.Quantidade.HasValue);

    item.RuleFor(i => i.ValorUnitario)
        .NotEmpty().WithMessage("ValorUnitario deve ser preenchido")
        .GreaterThan(0).WithMessage("ValorUnitario deve ser maior que zero")
        .When(i => i.ValorUnitario.HasValue);
});
    }
}
