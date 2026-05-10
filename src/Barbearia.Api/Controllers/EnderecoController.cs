using Barbearia.Application.Dtos.Endereco;
using Barbearia.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecoController(IEnderecoService enderecoService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EnderecoResponse>> Create([FromBody] EnderecoRequest request)
    {
        var result = await enderecoService.CreateAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<EnderecoResponse>> GetAll()
    {
        var result = await enderecoService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnderecoResponse>> GetById([FromRoute] int id)
    {
        var result = await enderecoService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<EnderecoResponse>> Update([FromRoute] int id, [FromBody] EnderecoUpdate update)
    {
        var result = await enderecoService.UpdateAsync(id, update);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await enderecoService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
}
