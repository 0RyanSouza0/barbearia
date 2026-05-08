using Barbearia.Application.Dtos.Pedido;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController(IPedidoService pedidoService) : ControllerBase
{
    [HttpPost]
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
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await pedidoService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("atualizar-quantidade-item-pedido/{id}")]
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
    public async Task<ActionResult<PedidoResponse>> AdicionarItemPedido([FromRoute] int id, [FromBody] AdicionarItemRequest request)
    {
        var result = await pedidoService.AddNovoItem(id, request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }



}
