
using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.ItemPedido;
using Barbearia.Application.Dtos.Pedido;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using Barbearia.Domain.Enuns;
using FluentValidation;

namespace Barbearia.Application.Service;

public class PedidoService(IPedidoRepository _repository, IProdutoRepository _produtoRepository, IValidator<PedidoRequest> _validatorRequest,
IValidator<AtualizarQuantidadeItemRequest> _validatorUpdate, IValidator<RemoverItemPedidoRequest> _validatorRemoverItem,
IValidator<AdicionarItemRequest> _validatorItemRequest, IClienteRepository _clienteRepository) : IPedidoService
{
    async Task<Result<PedidoResponse>> IPedidoService.CreateAsync(PedidoRequest request)
    {
        var resultValidator = _validatorRequest.Validate(request);
        if (!resultValidator.IsValid)
        {
            var erros = resultValidator.Errors.Select(e => e.ErrorMessage).ToList();

            return Result<PedidoResponse>.Failure(erros);
        }
        var cliente = await _clienteRepository.GetById(request.ClienteId);
        if (cliente is null)
        {
            return Result<PedidoResponse>.Failure("Cliente não encontrado");
        }

        var pedido = new Pedido(
            request.ClienteId
        );
        foreach (var item in request.Itens)
        {
            var produto = await _produtoRepository.GetById(item.ProdutoId);
            if (produto is null)
            {
                return Result<PedidoResponse>.Failure("Produto não encontrado");
            }
            pedido.AdicionarItem(produto.Id, item.Quantidade, produto.Valor);
        }
        pedido.CriadoEm = DateTime.UtcNow;
        await _repository.Add(pedido);
        await _repository.SaveChangesAsync();

        return Result<PedidoResponse>.Success(new PedidoResponse
        {
            Data = pedido.CriadoEm,
            DataAtualizacao = pedido.AtualizadoEm.Value,
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario
            })
        });
    }

    async Task<Result<string>> IPedidoService.DeleteAsync(int id)
    {
        var pedido = await _repository.GetById(id);

        if (pedido is null)
            return Result<string>.Failure("Pedido não encontrado");
        try
        {
            pedido.Cancelar();
            pedido.Ativo = false;
            pedido.AtualizadoEm = DateTime.UtcNow;
            await _repository.Update(pedido);
            await _repository.SaveChangesAsync();

            return Result<string>.Success("Pedido cancelado com sucesso");
        }
        catch (InvalidOperationException ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

    async Task<Result<IEnumerable<PedidoResponse>>> IPedidoService.GetAllAsync()
    {
        var pedidos = await _repository.GetAll();
        return Result<IEnumerable<PedidoResponse>>.Success(pedidos.Select(pedido => new PedidoResponse
        {
            Data = pedido.CriadoEm,
            DataAtualizacao = pedido.AtualizadoEm.Value,
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario
            })
        }));
    }

    async Task<Result<PedidoResponse>> IPedidoService.GetByIdAsync(int id)
    {
        var pedido = await _repository.GetById(id);
        if (pedido is null)
        {
            return Result<PedidoResponse>.Failure("Pedido não encontrado");
        }
        return Result<PedidoResponse>.Success(new PedidoResponse
        {
            Data = pedido.CriadoEm,
            DataAtualizacao = pedido.AtualizadoEm.Value,
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario
            })
        });
    }

    async Task<Result<PedidoResponse>> IPedidoService.AddNovoItem(int id, AdicionarItemRequest itemRequest)
    {
        var resultValidator = await _validatorItemRequest.ValidateAsync(itemRequest);
        if (!resultValidator.IsValid)
        {
            return Result<PedidoResponse>.Failure(resultValidator.Errors.First().ErrorMessage);
        }
        var pedido = await _repository.GetById(id);
        if (pedido is null)
        {
            return Result<PedidoResponse>.Failure("Pedido não encontrado");
        }
        try
        {
            var produto = await _produtoRepository.GetById(itemRequest.ProdutoId);
            if (produto is null)
            {
                return Result<PedidoResponse>.Failure("Produto não encontrado");
            }
            pedido.AdicionarItem(produto.Id, itemRequest.Quantidade, produto.Valor);
            pedido.AtualizadoEm = DateTime.UtcNow;
            await _repository.Update(pedido);
            await _repository.SaveChangesAsync();
            return Result<PedidoResponse>.Success(new PedidoResponse
            {
                Data = pedido.CriadoEm,
                DataAtualizacao = pedido.AtualizadoEm.Value,
                Id = pedido.Id,
                IdCliente = pedido.ClienteId,
                Status = pedido.Status,
                ValorTotal = pedido.ValorTotal,
                itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
                {
                    Id = i.Id,
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario
                })
            });

        }
        catch (InvalidOperationException ex)
        {
            return Result<PedidoResponse>.Failure(ex.Message);
        }
    }

    async Task<Result<PedidoResponse>> IPedidoService.UpdateQuantidadeItemPedido(int id, AtualizarQuantidadeItemRequest quantidadeItemRequest)
    {
        var resultValidator = await _validatorUpdate.ValidateAsync(quantidadeItemRequest);

        if (!resultValidator.IsValid)
            return Result<PedidoResponse>.Failure(resultValidator.Errors.First().ErrorMessage);

        var pedido = await _repository.GetById(id);

        if (pedido is null)
            return Result<PedidoResponse>.Failure("Pedido não encontrado");
        try
        {
            var produto = await _produtoRepository.GetById(quantidadeItemRequest.ProdutoId);

            if (produto is null)
                return Result<PedidoResponse>.Failure("Produto não encontrado");

            pedido.AtualizarQuantidadeItem(
                quantidadeItemRequest.ProdutoId,
                quantidadeItemRequest.Quantidade
            );
            pedido.AtualizadoEm = DateTime.UtcNow;
            await _repository.Update(pedido);
            await _repository.SaveChangesAsync();

        }
        catch (InvalidOperationException ex)
        {
            return Result<PedidoResponse>.Failure(ex.Message);
        }

        pedido.AtualizadoEm = DateTime.UtcNow;

        await _repository.Update(pedido);
        await _repository.SaveChangesAsync();

        return Result<PedidoResponse>.Success(new PedidoResponse
        {
            Data = pedido.CriadoEm,
            DataAtualizacao = pedido.AtualizadoEm.Value,
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario
            })
        });
    }

    async Task<Result<PedidoResponse>> IPedidoService.RemoverItemPedido(int id, RemoverItemPedidoRequest itemRequest)
    {
        var resultValidator = await _validatorRemoverItem.ValidateAsync(itemRequest);
        if (!resultValidator.IsValid)
        {
            return Result<PedidoResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var pedido = await _repository.GetById(id);
        if (pedido is null)
        {
            return Result<PedidoResponse>.Failure("Pedido nao encontrado");
        }
        var produto = await _produtoRepository.GetById(itemRequest.IdProduto);
        if (produto is null)
        {
            return Result<PedidoResponse>.Failure("Produto nao encontrado");
        }
        try
        {
            pedido.RemoverItem(produto.Id);
            pedido.AtualizadoEm = DateTime.UtcNow;
            await _repository.Update(pedido);
            await _repository.SaveChangesAsync();
            return Result<PedidoResponse>.Success(new PedidoResponse
            {
                Data = pedido.CriadoEm,
                DataAtualizacao = pedido.AtualizadoEm.Value,
                Id = pedido.Id,
                IdCliente = pedido.ClienteId,
                Status = pedido.Status,
                ValorTotal = pedido.ValorTotal,
                itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
                {
                    Id = i.Id,
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario
                })
            });
        }
        catch (InvalidOperationException ex)
        {
            return Result<PedidoResponse>.Failure(ex.Message);
        }

    }
}
