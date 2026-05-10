using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class PedidoRequestValidator : AbstractValidator<PedidoRequest>
{
    public PedidoRequestValidator()
    {
        RuleFor(p => p.ClienteId)
            .NotEmpty().WithMessage("ClienteId deve ser preenchido");
        RuleForEach(p => p.Itens).ChildRules(item =>
        {
            item.RuleFor(i => i.ProdutoId)
                .NotEmpty().WithMessage("ProdutoId deve ser preenchido");
            item.RuleFor(i => i.Quantidade)
                .NotEmpty().WithMessage("Quantidade deve ser preenchido")
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero");
        });
    }
}

