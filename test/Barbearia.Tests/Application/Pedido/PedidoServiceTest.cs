using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barbearia.Application.Dtos.ItemPedido;
using Barbearia.Application.Dtos.Pedido;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Application.Service;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Barbearia.Tests.Application.Pedido
{
    public class PedidoServiceTest
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IValidator<PedidoRequest>> _validatorMock;
        private readonly Mock<IValidator<AdicionarItemRequest>> _validatorAddItemMock;
        private readonly Mock<IValidator<RemoverItemPedidoRequest>> _validatorRemoverItemMock;
        private readonly Mock<IValidator<AtualizarQuantidadeItemRequest>> _validatorQuantidadeItemMock;
        private readonly IPedidoService pedidoService;

        public PedidoServiceTest()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _validatorMock = new Mock<IValidator<PedidoRequest>>();
            _validatorAddItemMock = new Mock<IValidator<AdicionarItemRequest>>();
            _validatorRemoverItemMock = new Mock<IValidator<RemoverItemPedidoRequest>>();
            _validatorQuantidadeItemMock = new Mock<IValidator<AtualizarQuantidadeItemRequest>>();
            pedidoService = new PedidoService(_pedidoRepositoryMock.Object, _produtoRepositoryMock.Object, _validatorMock.Object, _validatorQuantidadeItemMock.Object,
            _validatorRemoverItemMock.Object, _validatorAddItemMock.Object, _clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task DeveRetornar_ObjetoValidoPedido()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                Itens = new List<ItemPedidoRequest>
                {
                    new ItemPedidoRequest { ProdutoId = 1, Quantidade = 1 }
                }
            };

            _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _clienteRepositoryMock.Setup(c => c.GetById(request.ClienteId)).ReturnsAsync(new Domain.Entities.Cliente(
                "ryan",
                "11999999999",
                 "emailexiste@gmail.com",
                 "12345678"
            ));
            _produtoRepositoryMock.Setup(p => p.GetById(request.Itens.First().ProdutoId)).ReturnsAsync(new Domain.Entities.Produto(
                "produto",
                10.00m,
                10,
                "produto a",
                Domain.Entities.Categoria.Gel
            ));
            var result = await pedidoService.CreateAsync(request);

            result.IsFailure.Should().BeFalse();
            _pedidoRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task DeveRetornar_ObjetoInvalido_QuandoClienteNaoExistir()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                Itens = new List<ItemPedidoRequest>
                {
                    new ItemPedidoRequest { ProdutoId = 1, Quantidade = 1 }
                }
            };

            _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _clienteRepositoryMock.Setup(c => c.GetById(request.ClienteId)).ReturnsAsync((Domain.Entities.Cliente)null);


            var result = await pedidoService.CreateAsync(request);

            result.IsFailure.Should().BeTrue();
            _pedidoRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }


        [Fact]
        public async Task DeveRetornar_ObejetoInvalido_QuandoProdutoNaoExistir()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                Itens = new List<ItemPedidoRequest>
                {
                    new ItemPedidoRequest { ProdutoId = 1, Quantidade = 1 }
                }
            };

            _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _clienteRepositoryMock.Setup(c => c.GetById(request.ClienteId)).ReturnsAsync(new Domain.Entities.Cliente(
                "ryan",
                "11999999999",
                 "emailexiste@gmail.com",
                 "12345678"
            ));
            _produtoRepositoryMock.Setup(p => p.GetById(request.Itens.First().ProdutoId)).ReturnsAsync((Domain.Entities.Produto)null);
            var result = await pedidoService.CreateAsync(request);

            result.IsFailure.Should().BeTrue();
            _pedidoRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }


        [Fact]
        public async Task DeveRetornar_ObjetoInvalido_QuandoValidatorRetornarErro()
        {
            var request = new PedidoRequest
            {
                ClienteId = -1,
                Itens = new List<ItemPedidoRequest>
                {
                    new ItemPedidoRequest { ProdutoId = 1, Quantidade = 1 }
                }
            };

            _validatorMock.Setup(x => x.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
            {
                new ValidationFailure("ClienteId", "ClienteId deve ser maior que 0")
            }));
            var result = await pedidoService.CreateAsync(request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("ClienteId deve ser maior que 0");
            _pedidoRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

    }
}