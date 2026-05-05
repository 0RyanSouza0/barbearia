using Barbearia.Application.Dtos.Endereco;
using FluentValidation;

namespace Barbearia.Application.Validators.Endereco;

public class EnderecoUpdateValidator : AbstractValidator<EnderecoUpdate>
{
    public EnderecoUpdateValidator()
    {
        RuleFor(e => e.Logradouro)
            .NotEmpty().WithMessage("Logradouro deve ser preenchido")
            .When(e => e.Logradouro != null);
        RuleFor(e => e.Numero)
            .NotEmpty().WithMessage("Número deve ser preenchido")
            .When(e => e.Numero != null);
        RuleFor(e => e.Bairro)
            .NotEmpty().WithMessage("Bairro deve ser preenchido")
            .When(e => e.Bairro != null);
        RuleFor(e => e.Cidade)
            .NotEmpty().WithMessage("Cidade deve ser preenchido")
            .When(e => e.Cidade != null);
        RuleFor(e => e.Estado)
            .NotEmpty().WithMessage("Estado deve ser preenchido")
            .When(e => e.Estado != null);
        RuleFor(e => e.Cep)
            .NotEmpty().WithMessage("Cep deve ser preenchido")
            .MaximumLength(8).WithMessage("Cep deve ter no máximo 8 caracteres")
            .When(e => e.Cep != null);
    }
}
