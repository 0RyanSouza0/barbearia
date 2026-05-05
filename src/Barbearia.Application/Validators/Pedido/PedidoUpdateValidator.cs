using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class PedidoUpdateValidator : AbstractValidator<PedidoUpdate>
{
    public PedidoUpdateValidator()
    {
        RuleFor(p => p.ClienteId)
            .NotEmpty().WithMessage("ClienteId deve ser preenchido")
            .When(p => p.ClienteId != null);
    }
}
