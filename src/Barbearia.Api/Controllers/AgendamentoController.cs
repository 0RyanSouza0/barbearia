using Barbearia.Application.Dtos.Agendamento;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Enuns;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController(IAgendamentoService agendamentoService) : ControllerBase
{
    [HttpGet("status-agendamento")]
    public async Task<ActionResult<string>> StatusAgendamento()
    {
        var result = Enum.GetNames(typeof(StatusAgendamento));
        if (result is null)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<AgendamentoResponse>> Create([FromBody] AgendamentoRequest request)
    {
        var result = await agendamentoService.CreateAysnc(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<AgendamentoResponse>> GetAll()
    {
        var result = await agendamentoService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AgendamentoResponse>> GetById([FromRoute] int id)
    {
        var result = await agendamentoService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<AgendamentoResponse>> Update([FromRoute] int id, [FromBody] AgendamentoUpdate update)
    {
        var result = await agendamentoService.UpdateAsync(id, update);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet("cliente/{nome}")]
    public async Task<ActionResult<AgendamentoResponse>> GetByClientId([FromRoute] string nome)
    {
        var result = await agendamentoService.GetByClienteAsync(nome);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await agendamentoService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

}
