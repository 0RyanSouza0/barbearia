using System.Drawing;
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

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult());
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

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult());
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
    public async Task DeveSalvar_Cliente_ComSenhaCriptografada()
    {
        var request = new ClienteRequest
        {
            Email = "teste@gmail.com",
            Nome = "Ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        Domain.Entities.Cliente? clienteSalvo = null;

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ClienteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _clienteRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((Domain.Entities.Cliente?)null);

        _clienteRepositoryMock
            .Setup(x => x.Add(It.IsAny<Domain.Entities.Cliente>()))
            .Callback<Domain.Entities.Cliente>(cliente =>
            {
                clienteSalvo = cliente;
            })
            .ReturnsAsync((Domain.Entities.Cliente cliente) => cliente);

        var result = await clienteService.CreateAsync(request);

        result.IsSuccess.Should().BeTrue();

        clienteSalvo.Should().NotBeNull();
        clienteSalvo!.Senha.Should().NotBe(request.Senha);

        BCrypt.Net.BCrypt.Verify(request.Senha, clienteSalvo.Senha)
            .Should()
            .BeTrue();

        _clienteRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DevePreencher_CriadoEm_ComSucesso()
    {
        var request = new ClienteRequest
        {
            Email = "teste@gmail.com",
            Nome = "Ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        Domain.Entities.Cliente? clienteSalvo = null;

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ClienteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _clienteRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((Domain.Entities.Cliente?)null);

        _clienteRepositoryMock
            .Setup(x => x.Add(It.IsAny<Domain.Entities.Cliente>()))
            .Callback<Domain.Entities.Cliente>(cliente =>
            {
                clienteSalvo = cliente;
            })
            .ReturnsAsync((Domain.Entities.Cliente cliente) => cliente);

        var result = await clienteService.CreateAsync(request);

        result.IsSuccess.Should().BeTrue();

        clienteSalvo.Should().NotBeNull();
        clienteSalvo!.CriadoEm.Should().NotBe(default);

        BCrypt.Net.BCrypt.Verify(request.Senha, clienteSalvo.Senha)
            .Should()
            .BeTrue();

        _clienteRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_Erro_Quando_SenhaInvalida()
    {
        var request = new ClienteRequest
        {
            Email = "emailexiste@gmail.com",
            Nome = "ryan",
            Senha = "12345",
            Telefone = "11999999999"
        };

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
        {
            new ValidationFailure("Senha", "Senha deve ter pelo menos 8 caracteres")
        }));
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

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
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




    [Fact]
    public async Task DeveRetornar_ObjetoValidoUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = "teste@gmail.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };
        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
                (
                    "ryan",
                    "11999999999",
                     "emailexiste@gmail.com",
                     "12345678"

                ));
        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _clienteRepositoryMock.Setup(c => c.GetByEmailAsync(request.Email)).ReturnsAsync((Domain.Entities.Cliente)null);
        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeFalse();
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_Erro_Quando_ExistirEmailUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = "emailexiste@gmail.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };
        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryan",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ));

        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _clienteRepositoryMock.Setup(c => c.GetByEmailAsync(request.Email)).ReturnsAsync(new Domain.Entities.Cliente
        (
             "emailexiste@gmail.com",
              "ryan",
              "12345678",
             "11999999999"
        ));
        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeTrue();
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveAtualizar_SomenteCamposEnviados_NoUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = null,
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryandias",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ))
               ;

        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeFalse();
        result.Value.Email.Should().Be("emailexiste@gmail.com");
        result.Value.Nome.Should().Be(request.Nome);

        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DevePreencher_AtualizadoEm_NoUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = null,
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };
        Domain.Entities.Cliente? cliente = null;
        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryandias",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ))
               ;

        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _clienteRepositoryMock.Setup(c => c.Update(It.IsAny<Domain.Entities.Cliente>())).Callback((Domain.Entities.Cliente clienteExiste) =>
        {
            cliente = clienteExiste;
        }).ReturnsAsync((Domain.Entities.Cliente cliente) => cliente);
        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeFalse();
        cliente?.AtualizadoEm.Should().NotBeNull();
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }


    [Fact]
    public async Task DeveCriptografarSenha_NoUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = null,
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };
        Domain.Entities.Cliente? cliente = null;
        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryandias",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ))
               ;

        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _clienteRepositoryMock.Setup(c => c.Update(It.IsAny<Domain.Entities.Cliente>())).Callback((Domain.Entities.Cliente clienteExiste) =>
        {
            cliente = clienteExiste;
        }).ReturnsAsync((Domain.Entities.Cliente cliente) => cliente);
        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeFalse();
        cliente!.Senha.Should().NotBe(request.Senha);
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        cliente.Should().NotBeNull();
        BCrypt.Net.BCrypt.Verify(request.Senha, cliente!.Senha).Should().BeTrue();
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }




    [Fact]
    public async Task DeveRetornar_Erro_Quando_NaoExistirCliente()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = "emailexiste@gmail.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };

        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync((Domain.Entities.Cliente)null);
        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeTrue();
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }


    [Fact]
    public async Task DeveRetornar_Erro_Quando_SenhaInvalidaUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = "emailexiste@gmail.com",
            Nome = "ryan",
            Senha = "12345",
            Telefone = "11999999999"
        };
        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
                       (
                           "ryan",
                           "11999999999",
                            "emailexiste@gmail.com",
                            "12345678"

                       ));
        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
        {
            new ValidationFailure("Senha", "Senha deve ter pelo menos 8 caracteres")
        }));
        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeTrue();
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    [Fact]
    public async Task DeveRetornar_Erro_Quando_ValidatorFalharUpdate()
    {
        var id = 1;
        var request = new ClienteUpdate
        {
            Email = "emailexiste@errado.com",
            Nome = "ryan",
            Senha = "12345678",
            Telefone = "11999999999"
        };
        _clienteRepositoryMock.Setup(c => c.GetById(id)).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryan",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ));

        _validatorUpdateMock.Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
        {
            new ValidationFailure("Email", "Email deve ser válido")
        }));

        var result = await clienteService.UpdateAsync(id, request);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Email deve ser válido");
        _clienteRepositoryMock.Verify(x => x.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveRemover_Cliente_ComSucesso()
    {

        Domain.Entities.Cliente? cliente = null;
        _clienteRepositoryMock.Setup(c => c.GetById(It.IsAny<int>())).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryan",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ));
        _clienteRepositoryMock.Setup(c => c.Update(It.IsAny<Domain.Entities.Cliente>())).Callback<Domain.Entities.Cliente>(c =>
        {
            cliente = c;
        }).ReturnsAsync(new Domain.Entities.Cliente
               (
                   "ryan",
                   "11999999999",
                    "emailexiste@gmail.com",
                    "12345678"

               ));
        var result = await clienteService.DeleteAsync(1);

        result.IsFailure.Should().BeFalse();
        cliente?.Ativo.Should().BeFalse();
        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_Erro_Quando_DeletarClienteInexistente()
    {
        _clienteRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Cliente?)null);

        var result = await clienteService.DeleteAsync(1);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Cliente não encontrado");

        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Never);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveRetornar_Cliente_Quando_BuscarPorId()
    {
        var cliente = new Domain.Entities.Cliente(
            "Ryan",
            "11999999999",
            "teste@gmail.com",
            "12345678"
        );

        _clienteRepositoryMock
            .Setup(x => x.GetById(1))
            .ReturnsAsync(cliente);

        var result = await clienteService.GetByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Nome.Should().Be("Ryan");
        result.Value.Email.Should().Be("teste@gmail.com");

        _clienteRepositoryMock.Verify(x => x.GetById(1), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_Erro_Quando_BuscarPorId_NaoEncontrarCliente()
    {
        _clienteRepositoryMock
            .Setup(x => x.GetById(1))
            .ReturnsAsync((Domain.Entities.Cliente?)null);

        var result = await clienteService.GetByIdAsync(1);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Cliente não encontrado");

        _clienteRepositoryMock.Verify(x => x.GetById(1), Times.Once);
    }


    [Fact]
    public async Task DeveRetornar_Cliente_Quando_BuscarPorEmail()
    {
        var email = "teste@gmail.com";

        var cliente = new Domain.Entities.Cliente(
            "Ryan",
            "11999999999",
            email,
            "12345678"
        );

        _clienteRepositoryMock
            .Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(cliente);

        var result = await clienteService.GetByEmailAsync(email);

        result.IsSuccess.Should().BeTrue();
        result.Value.Nome.Should().Be("Ryan");
        result.Value.Email.Should().Be(email);

        _clienteRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_Erro_Quando_BuscarPorEmail_NaoEncontrarCliente()
    {
        var email = "naoexiste@gmail.com";

        _clienteRepositoryMock
            .Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((Domain.Entities.Cliente?)null);

        var result = await clienteService.GetByEmailAsync(email);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Cliente não encontrado");

        _clienteRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_ListaDeClientes_ComSucesso()
    {
        var clientes = new List<Domain.Entities.Cliente>
    {
        new("Ryan", "11999999999", "ryan@gmail.com", "12345678"),
        new("Joao", "11888888888", "joao@gmail.com", "12345678")
    };

        _clienteRepositoryMock
            .Setup(x => x.GetAll())
            .ReturnsAsync(clientes);

        var result = await clienteService.GetAllAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);

        _clienteRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task DeveRetornar_ListaDeClientesRelatorio_ComSucesso()
    {
        var clientes = new List<Domain.Entities.Cliente>
    {
        new("Ryan", "11999999999", "ryan@gmail.com", "12345678"),
        new("Joao", "11888888888", "joao@gmail.com", "12345678")
    };

        _clienteRepositoryMock
            .Setup(x => x.GetAllClientesRelatorio())
            .ReturnsAsync(clientes);

        var result = await clienteService.GetAllRelatorioAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);

        _clienteRepositoryMock.Verify(x => x.GetAllClientesRelatorio(), Times.Once);
    }

    [Fact]
    public async Task DevePermitir_Atualizar_ComMesmoEmail_DoProprioCliente()
    {
        var id = 1;

        var clienteExistente = new Domain.Entities.Cliente(
            "Ryan",
            "11999999999",
            "teste@gmail.com",
            "12345678"
        );

        clienteExistente.Id = id;

        var request = new ClienteUpdate
        {
            Nome = "Ryan Atualizado",
            Email = "teste@gmail.com",
            Senha = null,
            Telefone = null
        };

        _clienteRepositoryMock
            .Setup(x => x.GetById(id))
            .ReturnsAsync(clienteExistente);

        _validatorUpdateMock
            .Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdate>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _clienteRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(clienteExistente);

        var result = await clienteService.UpdateAsync(id, request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("teste@gmail.com");
        result.Value.Nome.Should().Be("Ryan Atualizado");

        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Cliente>()), Times.Once);
        _clienteRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}


