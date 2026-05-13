using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class RemoverItemPedidoRequestValidator : AbstractValidator<RemoverItemPedidoRequest>
{
    public RemoverItemPedidoRequestValidator()
    {
        RuleFor(i => i.IdProduto)
            .NotEmpty().WithMessage("IdProduto deve ser preenchido")
            .GreaterThan(0).WithMessage("IdProduto deve ser maior que zero");
    }
}
