using Barbearia.Application.Dtos.Cliente;
using Barbearia.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController(IClienteService clienteService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ClienteResponse>> Create([FromBody] ClienteRequest request)
    {
        var result = await clienteService.CreateAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClienteResponse>> GetAll()
    {
        var result = await clienteService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("relatorio")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ClienteResponse>>> GetAllClientesRelatorio()
    {
        var result = await clienteService.GetAllRelatorioAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClienteResponse>> GetById([FromRoute] int id)
    {
        var result = await clienteService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClienteResponse>> GetByEmail([FromRoute] string email)
    {
        var result = await clienteService.GetByEmailAsync(email);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await clienteService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<ClienteResponse>> Update([FromRoute] int id, [FromBody] ClienteUpdate update)
    {
        var result = await clienteService.UpdateAsync(id, update);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

}
