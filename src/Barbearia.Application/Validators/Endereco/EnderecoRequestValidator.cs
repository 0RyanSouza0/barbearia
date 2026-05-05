using Barbearia.Application.Dtos.Barbeiro;
using FluentValidation;

namespace Barbearia.Application.Validators.Endereco;

public class EnderecoRequestValidator : AbstractValidator<EnderecoRequest>
{
    public EnderecoRequestValidator()
    {
        RuleFor(e => e.Logradouro)
            .NotEmpty().WithMessage("Logradouro deve ser preenchido");
        RuleFor(e => e.Numero)
            .NotEmpty().WithMessage("Número deve ser preenchido");
        RuleFor(e => e.Bairro)
            .NotEmpty().WithMessage("Bairro deve ser preenchido");
        RuleFor(e => e.Cidade)
            .NotEmpty().WithMessage("Cidade deve ser preenchido");
        RuleFor(e => e.Estado)
            .NotEmpty().WithMessage("Estado deve ser preenchido");
        RuleFor(e => e.Cep)
            .NotEmpty().WithMessage("Cep deve ser preenchido")
            .MaximumLength(8).WithMessage("Cep deve ter no máximo 8 caracteres");

    }
}
