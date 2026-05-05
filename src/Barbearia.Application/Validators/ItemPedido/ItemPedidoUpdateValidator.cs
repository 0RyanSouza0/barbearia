using Barbearia.Application.Dtos.ItemPedido;
using FluentValidation;

namespace Barbearia.Application.Validators.ItemPedido;

public class ItemPedidoUpdateValidator : AbstractValidator<ItemPedidoUpdate>
{
    public ItemPedidoUpdateValidator()
    {
        RuleFor(i => i.PedidoId)
            .NotEmpty().WithMessage("PedidoId deve ser preenchido")
            .When(i => i.PedidoId != null);
        RuleFor(i => i.ProdutoId)
            .NotEmpty().WithMessage("ProdutoId deve ser preenchido")
            .When(i => i.ProdutoId != null);
        RuleFor(i => i.Quantidade)
            .NotEmpty().WithMessage("Quantidade deve ser preenchido")
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero")
            .When(i => i.Quantidade != null);
        RuleFor(i => i.ValorUnitario)
            .NotEmpty().WithMessage("ValorUnitario deve ser preenchido")
            .GreaterThan(0).WithMessage("ValorUnitario deve ser maior que zero")
            .When(i => i.ValorUnitario != null);

    }
}
