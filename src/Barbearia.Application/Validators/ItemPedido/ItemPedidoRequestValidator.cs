using Barbearia.Application.Dtos.ItemPedido;
using FluentValidation;

namespace Barbearia.Application.Validators.ItemPedido;

public class ItemPedidoRequestValidator : AbstractValidator<ItemPedidoRequest>
{
    public ItemPedidoRequestValidator()
    {
        RuleFor(i => i.PedidoId)
            .NotEmpty().WithMessage("PedidoId deve ser preenchido");
        RuleFor(i => i.ProdutoId)
            .NotEmpty().WithMessage("ProdutoId deve ser preenchido");
        RuleFor(i => i.Quantidade)
            .NotEmpty().WithMessage("Quantidade deve ser preenchido")
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero");
        RuleFor(i => i.ValorUnitario)
            .NotEmpty().WithMessage("ValorUnitario deve ser preenchido")
            .GreaterThan(0).WithMessage("ValorUnitario deve ser maior que zero");

    }
}
