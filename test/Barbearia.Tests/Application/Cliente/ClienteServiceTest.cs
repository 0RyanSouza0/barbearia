using System.Reflection.PortableExecutable;
using Barbearia.Application.Dtos.Cliente;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Application.Service;
using Barbearia.Domain.Enuns;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Barbearia.Tests.Application.Cliente;

public class ClienteServiceTest
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IValidator<ClienteRequest>> _validatorMock;
    private readonly Mock<IValidator<ClienteUpdate>> _validatorUpdateMock;
    private readonly IClienteService clienteService;
    public ClienteServiceTest()
    {
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _validatorMock = new Mock<IValidator<ClienteRequest>>();
        _validatorUpdateMock = new Mock<IValidator<ClienteUpdate>>();
        clienteService = new ClienteService(_clienteRepositoryMock.Object, _validatorMock.Object, _validatorUpdateMock.Object);
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

        _validatorMock.Setup(x => x.ValidateAsync(request)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _clienteRepositoryMock.Setup(c => c.GetByEmailAsync(request.Email)).ReturnsAsync((Domain.Entities.Cliente)null);
        var result = await clienteService.CreateAsync(request);

        result.IsFailure.Should().BeFalse();
        _clienteRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_Erro_Quando_ExistirEmail()
    {
        var request = new ClienteRequest
        {
            Email = "emailexiste@gmail.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        _validatorMock.Setup(x => x.ValidateAsync(request)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _clienteRepositoryMock.Setup(c => c.GetByEmailAsync(request.Email)).ReturnsAsync(new Domain.Entities.Cliente
        (
             "emailexiste@gmail.com",
              "ryan",
              "12345678",
             "11999999999"
        ));
        var result = await clienteService.CreateAsync(request);

        result.IsFailure.Should().BeTrue();
        _clienteRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    [Fact]
    public async Task DeveRetornar_Erro_Quando_ValidatorFalhar()
    {
        var request = new ClienteRequest
        {
            Email = "emailexiste@errado.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
        {
            new ValidationFailure("Email", "Email deve ser preenchido")
        }));
        _clienteRepositoryMock.Setup(c => c.GetByEmailAsync(request.Email)).ReturnsAsync(new Domain.Entities.Cliente
        (
             "emailexiste@gmail.com",
              "ryan",
              "12345678",
             "11999999999"
        ));
        var result = await clienteService.CreateAsync(request);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Email deve ser preenchido");
        _clienteRepositoryMock.Verify(x => x.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

}


