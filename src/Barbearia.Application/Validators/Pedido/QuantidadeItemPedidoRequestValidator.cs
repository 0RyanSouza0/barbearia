using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class QuantidadeItemPedidoRequestValidator : AbstractValidator<AtualizarQuantidadeItemRequest>
{
    public QuantidadeItemPedidoRequestValidator()
    {
        RuleFor(q => q.ProdutoId)
            .NotEmpty().WithMessage("IdProduto deve ser preenchido")
            .GreaterThan(0).WithMessage("IdProduto deve ser maior que zero");
        RuleFor(q => q.Quantidade)
            .NotEmpty().WithMessage("Quantidade deve ser preenchido")
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero");

    }
}
