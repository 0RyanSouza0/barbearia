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

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PedidoRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
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

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PedidoRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
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

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PedidoRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
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

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<PedidoRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
            {
                new ValidationFailure("ClienteId", "ClienteId deve ser maior que 0")
            }));
            var result = await pedidoService.CreateAsync(request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("ClienteId deve ser maior que 0");
            _pedidoRepositoryMock.Verify(x => x.Add(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveRetornar_Pedido_Quando_BuscarPorId()
        {
            var pedido = new Domain.Entities.Pedido(1);
            pedido.AdicionarItem(pedido.Id, 1, 2, 10m);

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            var result = await pedidoService.GetByIdAsync(1);

            result.IsSuccess.Should().BeTrue();
            result.Value.IdCliente.Should().Be(1);
            result.Value.ItemPedidos.Should().HaveCount(1);

            _pedidoRepositoryMock.Verify(x => x.GetById(1), Times.Once);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_BuscarPorId_NaoEncontrarPedido()
        {
            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync((Domain.Entities.Pedido?)null);

            var result = await pedidoService.GetByIdAsync(1);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Pedido não encontrado");

            _pedidoRepositoryMock.Verify(x => x.GetById(1), Times.Once);
        }

        [Fact]
        public async Task DeveRetornar_ListaDePedidos_ComSucesso()
        {
            var pedido1 = new Domain.Entities.Pedido(1);
            pedido1.AdicionarItem(pedido1.Id, 1, 2, 10m);

            var pedido2 = new Domain.Entities.Pedido(2);
            pedido2.AdicionarItem(pedido2.Id, 2, 1, 20m);

            var pedidos = new List<Domain.Entities.Pedido>
    {
        pedido1,
        pedido2
    };

            _pedidoRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(pedidos);

            var result = await pedidoService.GetAllAsync();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);

            _pedidoRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task DeveCancelar_Pedido_ComSucesso()
        {
            Domain.Entities.Pedido? pedidoAtualizado = null;

            var pedido = new Domain.Entities.Pedido(1);

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            _pedidoRepositoryMock
                .Setup(x => x.Update(It.IsAny<Domain.Entities.Pedido>()))
                .Callback<Domain.Entities.Pedido>(p =>
                {
                    pedidoAtualizado = p;
                })
                .ReturnsAsync((Domain.Entities.Pedido p) => p);

            var result = await pedidoService.DeleteAsync(1);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("Pedido cancelado com sucesso");

            pedidoAtualizado.Should().NotBeNull();
            pedidoAtualizado!.Ativo.Should().BeFalse();
            pedidoAtualizado.AtualizadoEm.Should().NotBeNull();

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_CancelarPedidoInexistente()
        {
            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync((Domain.Entities.Pedido?)null);

            var result = await pedidoService.DeleteAsync(1);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Pedido não encontrado");

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveAdicionar_NovoItem_AoPedido_ComSucesso()
        {
            var pedido = new Domain.Entities.Pedido(1);

            var request = new AdicionarItemRequest
            {
                ProdutoId = 1,
                Quantidade = 2
            };

            var produto = new Domain.Entities.Produto(
                "Pomada",
                15m,
                10,
                "Pomada modeladora",
                Domain.Entities.Categoria.Pomada
            );

            produto.Id = 1;

            _validatorAddItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<AdicionarItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            _produtoRepositoryMock
                .Setup(x => x.GetById(request.ProdutoId))
                .ReturnsAsync(produto);

            var result = await pedidoService.AddNovoItem(1, request);

            result.IsSuccess.Should().BeTrue();
            result.Value.ItemPedidos.Should().HaveCount(1);
            result.Value.ValorTotal.Should().Be(30m);

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_AdicionarItem_ValidatorFalhar()
        {
            var request = new AdicionarItemRequest
            {
                ProdutoId = 1,
                Quantidade = 0
            };

            _validatorAddItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<AdicionarItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[]
                {
            new ValidationFailure("Quantidade", "Quantidade deve ser maior que 0")
                }));

            var result = await pedidoService.AddNovoItem(1, request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Quantidade deve ser maior que 0");

            _pedidoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_AdicionarItem_PedidoNaoExistir()
        {
            var request = new AdicionarItemRequest
            {
                ProdutoId = 1,
                Quantidade = 2
            };

            _validatorAddItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<AdicionarItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync((Domain.Entities.Pedido?)null);

            var result = await pedidoService.AddNovoItem(1, request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Pedido não encontrado");

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_AdicionarItem_ProdutoNaoExistir()
        {
            var pedido = new Domain.Entities.Pedido(1);

            var request = new AdicionarItemRequest
            {
                ProdutoId = 1,
                Quantidade = 2
            };

            _validatorAddItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<AdicionarItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            _produtoRepositoryMock
                .Setup(x => x.GetById(request.ProdutoId))
                .ReturnsAsync((Domain.Entities.Produto?)null);

            var result = await pedidoService.AddNovoItem(1, request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Produto não encontrado");

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveAtualizar_QuantidadeDoItem_ComSucesso()
        {
            var pedido = new Domain.Entities.Pedido(1);
            pedido.AdicionarItem(pedido.Id, 1, 2, 10m);

            var request = new AtualizarQuantidadeItemRequest
            {
                ProdutoId = 1,
                Quantidade = 5
            };

            var produto = new Domain.Entities.Produto(
                "Pomada",
                10m,
                10,
                "Pomada modeladora",
                Domain.Entities.Categoria.Pomada
            );

            produto.Id = 1;

            _validatorQuantidadeItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<AtualizarQuantidadeItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            _produtoRepositoryMock
                .Setup(x => x.GetById(request.ProdutoId))
                .ReturnsAsync(produto);

            var result = await pedidoService.UpdateQuantidadeItemPedido(1, request);

            result.IsSuccess.Should().BeTrue();
            result.Value.ItemPedidos.First().Quantidade.Should().Be(7);
            result.Value.ValorTotal.Should().Be(70m);

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_AtualizarQuantidade_ItemNaoExistirNoPedido()
        {
            var pedido = new Domain.Entities.Pedido(1);

            var request = new AtualizarQuantidadeItemRequest
            {
                ProdutoId = 1,
                Quantidade = 5
            };

            var produto = new Domain.Entities.Produto(
                "Pomada",
                10m,
                10,
                "Pomada modeladora",
                Domain.Entities.Categoria.Pomada
            );

            produto.Id = 1;

            _validatorQuantidadeItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<AtualizarQuantidadeItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            _produtoRepositoryMock
                .Setup(x => x.GetById(request.ProdutoId))
                .ReturnsAsync(produto);

            var result = await pedidoService.UpdateQuantidadeItemPedido(1, request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Item nao encontrado no pedido");

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveRemover_ItemDoPedido_ComSucesso()
        {
            var pedido = new Domain.Entities.Pedido(1);
            pedido.AdicionarItem(pedido.Id, 1, 2, 10m);

            var request = new RemoverItemPedidoRequest
            {
                IdProduto = 1
            };

            var produto = new Domain.Entities.Produto(
                "Pomada",
                10m,
                10,
                "Pomada modeladora",
                Domain.Entities.Categoria.Pomada
            );

            produto.Id = 1;

            _validatorRemoverItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<RemoverItemPedidoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(pedido);

            _produtoRepositoryMock
                .Setup(x => x.GetById(request.IdProduto))
                .ReturnsAsync(produto);

            var result = await pedidoService.RemoverItemPedido(1, request);

            result.IsSuccess.Should().BeTrue();
            result.Value.ItemPedidos.Should().BeEmpty();
            result.Value.ValorTotal.Should().Be(0);

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_RemoverItem_ValidatorFalhar()
        {
            var request = new RemoverItemPedidoRequest
            {
                IdProduto = 0
            };

            _validatorRemoverItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<RemoverItemPedidoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[]
                {
            new ValidationFailure("IdProduto", "Produto inválido")
                }));

            var result = await pedidoService.RemoverItemPedido(1, request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Produto inválido");

            _pedidoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveRetornar_Erro_Quando_RemoverItem_PedidoNaoExistir()
        {
            var request = new RemoverItemPedidoRequest
            {
                IdProduto = 1
            };

            _validatorRemoverItemMock
                .Setup(x => x.ValidateAsync(It.IsAny<RemoverItemPedidoRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _pedidoRepositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync((Domain.Entities.Pedido?)null);

            var result = await pedidoService.RemoverItemPedido(1, request);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain("Pedido nao encontrado");

            _pedidoRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
            _pedidoRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveRetornar_ListaDePedidosRelatorio_ComSucesso()
        {
            var pedido1 = new Domain.Entities.Pedido(1);
            pedido1.AdicionarItem(pedido1.Id, 1, 2, 10m);

            var pedido2 = new Domain.Entities.Pedido(2);
            pedido2.AdicionarItem(pedido2.Id, 2, 1, 20m);

            var pedidos = new List<Domain.Entities.Pedido>
    {
        pedido1,
        pedido2
    };

            _pedidoRepositoryMock
                .Setup(x => x.GetAllProdutosRelatorio())
                .ReturnsAsync(pedidos);

            var result = await pedidoService.GetAllProdutosRelatorio();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);

            _pedidoRepositoryMock.Verify(x => x.GetAllProdutosRelatorio(), Times.Once);
        }
    }
}