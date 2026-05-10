using System.Runtime.InteropServices.Marshalling;
using Barbearia.Application.Dtos.Cliente;
using Barbearia.Application.Validators.Cliente;
using FluentAssertions;

namespace Barbearia.Tests.Application.Cliente;

public class ValidatorTest
{
    private readonly ClienteRequestValidator clienteRequestValidator;
    private readonly ClienteUpdateValidator clienteUpdateValidator;

    public ValidatorTest()
    {
        clienteRequestValidator = new ClienteRequestValidator();
        clienteUpdateValidator = new ClienteUpdateValidator();
    }
    [Fact]
    public async Task DeveRetornar_ObjetoValido()
    {
        var request = new ClienteRequest
        {
            Email = "teste@gmail.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        var result = await clienteRequestValidator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();

    }

    [Theory]
    [InlineData("teste", "ryan", "12345678", "11999999999")]
    [InlineData("teste@gmail.com", "", "12345678", "11999999999")]
    [InlineData("teste@gmail.com", "ryan", "23", "11999999999")]
    [InlineData("teste@gmail.com", "ryan", "12345678", "119999999995554")]
    public async Task DeveRetornar_ObjetoInvalido(string email, string nome, string senha, string telefone)
    {
        var request = new ClienteRequest
        {
            Email = email,
            Nome = nome,
            Senha = senha,
            Telefone = telefone
        };

        var result = await clienteRequestValidator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();

    }



    [Fact]
    public async Task DeveRetornar_ObjetoValido_Update()
    {
        var request = new ClienteUpdate
        {
            Email = "teste@gmail.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        var result = await clienteUpdateValidator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();

    }

    [Theory]
    [InlineData("teste", "ryan", "12345678", "11999999999")]
    [InlineData("teste@gmail.com", "", "12345678", "11999999999")]
    [InlineData("teste@gmail.com", "ryan", "1234567", "11999999999")]
    [InlineData("teste@gmail.com", "ryan", "12345678", "119999999995554")]
    public async Task DeveRetornar_ObjetoInvalido_Update(string email, string nome, string senha, string telefone)
    {
        var request = new ClienteUpdate
        {
            Email = email,
            Nome = nome,
            Senha = senha,
            Telefone = telefone
        };

        var result = await clienteUpdateValidator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();

    }

}

