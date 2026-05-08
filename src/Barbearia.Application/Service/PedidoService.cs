
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
IValidator<PedidoUpdate> _validatorUpdate, IClienteRepository _clienteRepository) : IPedidoService
{
    async Task<Result<PedidoResponse>> IPedidoService.CreateAsync(PedidoRequest request)
    {
        var resultValidator = _validatorRequest.Validate(request);
        if (!resultValidator.IsValid)
        {
            var erros = resultValidator.Errors.Select(e => e.ErrorMessage).ToList();

            return Result<PedidoResponse>.Failure(erros);
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
            pedido.AdicionarItem(item.ProdutoId, item.Quantidade, item.ValorUnitario);
        }
        pedido.CriadoEm = DateTime.UtcNow;
        await _repository.Add(pedido);
        await _repository.SaveChangesAsync();

        return Result<PedidoResponse>.Success(new PedidoResponse
        {
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Data = pedido.CriadoEm = DateTime.UtcNow,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ProdutoId = i.ProdutoId
            })
        });
    }

    async Task<Result<string>> IPedidoService.DeleteAsync(int id)
    {
        var pedido = await _repository.GetById(id);

        if (pedido is null)
            return Result<string>.Failure("Pedido não encontrado");
        if (pedido.Status == StatusPedido.Enviado)
        {
            return Result<string>.Failure("Pedido enviado não pode ser cancelado");
        }
        try
        {
            pedido.Cancelar();
            pedido.Ativo = false;
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
        return Result<IEnumerable<PedidoResponse>>.Success(pedidos.Select(p => new PedidoResponse
        {
            Id = p.Id,
            IdCliente = p.ClienteId,
            Data = p.CriadoEm = DateTime.UtcNow,
            Status = p.Status,
            ValorTotal = p.ValorTotal,
            itemPedidos = p.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ProdutoId = i.ProdutoId
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
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Data = pedido.CriadoEm = DateTime.UtcNow,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ProdutoId = i.ProdutoId
            })
        });
    }

    async Task<Result<PedidoResponse>> IPedidoService.AddItem(int id, PedidoUpdate pedidoUpdate)
    {
        var resultValidator = await _validatorUpdate.ValidateAsync(pedidoUpdate);
        if (!resultValidator.IsValid)
        {
            return Result<PedidoResponse>.Failure(resultValidator.Errors.First().ErrorMessage);
        }
        var pedido = await _repository.GetById(id);
        if (pedido is null)
        {
            return Result<PedidoResponse>.Failure("Pedido não encontrado");
        }
        if (pedido.Status == StatusPedido.Cancelado || pedido.Status == StatusPedido.Enviado)
        {
            return Result<PedidoResponse>.Failure("Pedido ja cancelado ou enviado");
        }
        try
        {
            foreach (var item in pedidoUpdate.Itens)
            {
                var produto = await _produtoRepository.GetById(item.ProdutoId.Value);
                if (produto is null)
                {
                    return Result<PedidoResponse>.Failure("Produto não encontrado");
                }
                pedido.AdicionarItem(item.ProdutoId.Value, item.Quantidade.Value, item.ValorUnitario.Value);
            }
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
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Data = pedido.CriadoEm,
            Status = pedido.Status,
            ValorTotal = pedido.ValorTotal,
            itemPedidos = pedido.Itens.Select(i => new ItemPedidoResponse
            {
                Id = i.Id,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
            })
        });
    }

    async Task<Result<PedidoResponse>> IPedidoService.UpdateQuantidade(int id, PedidoUpdate pedidoUpdate)
    {
        var resultValidator = await _validatorUpdate.ValidateAsync(pedidoUpdate);

        if (!resultValidator.IsValid)
            return Result<PedidoResponse>.Failure(resultValidator.Errors.First().ErrorMessage);

        var pedido = await _repository.GetById(id);

        if (pedido is null)
            return Result<PedidoResponse>.Failure("Pedido não encontrado");

        if (pedido.Status == StatusPedido.Cancelado || pedido.Status == StatusPedido.Enviado)
            return Result<PedidoResponse>.Failure("Pedido já foi cancelado ou enviado");

        try
        {
            foreach (var item in pedidoUpdate.Itens)
            {
                var produto = await _produtoRepository.GetById(item.ProdutoId.Value);

                if (produto is null)
                    return Result<PedidoResponse>.Failure("Produto não encontrado");

                pedido.AtualizarQuantidadeItem(
                    item.ProdutoId.Value,
                    item.Quantidade.Value
                );
            }
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
            Id = pedido.Id,
            IdCliente = pedido.ClienteId,
            Data = pedido.CriadoEm,
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

}
