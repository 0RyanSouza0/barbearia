using Barbearia.Application.Dtos.Pedido;
using FluentValidation;

namespace Barbearia.Application.Validators.Pedido;

public class PedidoRequestValidator : AbstractValidator<PedidoRequest>
{
    public PedidoRequestValidator()
    {
        RuleFor(p => p.ClienteId)
            .NotEmpty().WithMessage("ClienteId deve ser preenchido");
    }
}
