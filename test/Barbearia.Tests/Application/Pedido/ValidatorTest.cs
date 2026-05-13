using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barbearia.Application.Dtos.ItemPedido;
using Barbearia.Application.Dtos.Pedido;
using Barbearia.Application.Validators.Pedido;
using FluentAssertions;

namespace Barbearia.Tests.Application.Pedido
{
    public class ValidatorTest
    {
        private readonly PedidoRequestValidator _validatorPedido;
        private readonly AdicionarNovoItemRequestValidator _validatorAddItem;
        private readonly QuantidadeItemPedidoRequestValidator _validatorQuantidadeItem;
        private readonly RemoverItemPedidoRequestValidator _validatorRemoverItem;

        public ValidatorTest()
        {
            _validatorAddItem = new AdicionarNovoItemRequestValidator();
            _validatorPedido = new PedidoRequestValidator();
            _validatorQuantidadeItem = new QuantidadeItemPedidoRequestValidator();
            _validatorRemoverItem = new RemoverItemPedidoRequestValidator();
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

            var result = await _validatorPedido.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(-1, 1, 10)]
        [InlineData(1, -11, 10)]
        [InlineData(1, 1, -10)]
        [InlineData(null, 1, -10)]
        [InlineData(1, null, -10)]
        [InlineData(1, 1, null)]
        public async Task DeveRetornar_ObjetoInvalidoPedido(int id, int produtoId, int quantidade)
        {
            var request = new PedidoRequest
            {
                ClienteId = id,
                Itens = new List<ItemPedidoRequest>
                {
                    new ItemPedidoRequest { ProdutoId = produtoId, Quantidade = quantidade }
                }
            };

            var result = await _validatorPedido.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Any().Should().BeTrue();
        }



        [Fact]
        public async Task DeveRetornar_ObjetoValidoAddItem()
        {
            var request = new AdicionarItemRequest
            {
                ProdutoId = 1,
                Quantidade = 1

            };

            var result = await _validatorAddItem.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(-11, 10)]
        [InlineData(1, -10)]
        [InlineData(null, -10)]
        [InlineData(1, null)]
        public async Task DeveRetornar_ObjetoInvalidoAddItem(int produtoId, int quantidade)
        {
            var request = new AdicionarItemRequest
            {
                ProdutoId = produtoId,
                Quantidade = quantidade
            };

            var result = await _validatorAddItem.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Any().Should().BeTrue();
        }


        [Fact]
        public async Task DeveRetornar_ObjetoValidoRemoverItem()
        {
            var request = new RemoverItemPedidoRequest
            {
                IdProduto = 1
            };

            var result = await _validatorRemoverItem.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Theory]

        [InlineData(-1)]
        [InlineData(null)]
        public async Task DeveRetornar_ObjetoInvalidoRemoverItem(int produtoId)
        {
            var request = new RemoverItemPedidoRequest
            {
                IdProduto = produtoId,
            };

            var result = await _validatorRemoverItem.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Any().Should().BeTrue();
        }


        [Fact]
        public async Task DeveRetornar_ObjetoValidoQuantidadeItem()
        {
            var request = new AtualizarQuantidadeItemRequest
            {
                ProdutoId = 1,
                Quantidade = 1
            };

            var result = await _validatorQuantidadeItem.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Theory]

        [InlineData(1, -10)]
        [InlineData(-2, 10)]
        [InlineData(null, 10)]
        [InlineData(2, null)]
        public async Task DeveRetornar_ObjetoInvalidoQuantidadeItem(int produtoId, int quantidade)
        {
            var request = new AtualizarQuantidadeItemRequest
            {
                ProdutoId = produtoId,
                Quantidade = quantidade
            };

            var result = await _validatorQuantidadeItem.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Any().Should().BeTrue();
        }
    }
}