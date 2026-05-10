using Barbearia.Application.Dtos.ItemPedido;
using Barbearia.Application.Dtos.Pedido;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using Barbearia.Domain.Enuns;
using Barbearia.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController(IPedidoService pedidoService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<PedidoResponse>> Create([FromBody] PedidoRequest request)
    {
        var result = await pedidoService.CreateAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<PedidoResponse>> GetAll()
    {
        var result = await pedidoService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<PedidoResponse>> GetById([FromRoute] int id)
    {
        var result = await pedidoService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await pedidoService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("deletar-item-pedido/{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<string>> DeleteItemPedido([FromRoute] int id, [FromBody] RemoverItemPedidoRequest idProduto)
    {
        var result = await pedidoService.RemoverItemPedido(id, idProduto);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPut("atualizar-quantidade-item-pedido/{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<PedidoResponse>> UpdateQuantidadeItemPedido([FromRoute] int id, [FromBody] AtualizarQuantidadeItemRequest request)
    {
        var result = await pedidoService.UpdateQuantidadeItemPedido(id, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpPatch("adicionar-item/{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<PedidoResponse>> AdicionarItemPedido([FromRoute] int id, [FromBody] AdicionarItemRequest request)
    {
        var result = await pedidoService.AddNovoItem(id, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet("todos-pedidos")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<PedidoResponse>>> GetAllPedidosRelatorio()
    {
        var result = await pedidoService.GetAllProdutosRelatorio();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }




}
