using Barbearia.Application.Dtos.Servicos;
using Barbearia.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicosController(IServicosService servicosService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServicosResponse>> Create([FromBody] ServicosRequest request)
    {
        var result = await servicosService.CreateAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<ServicosResponse>> GetAll()
    {
        var result = await servicosService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<ServicosResponse>> GetById([FromRoute] int id)
    {
        var result = await servicosService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServicosResponse>> Update([FromRoute] int id, [FromBody] ServicosUpdate update)
    {
        var result = await servicosService.UpdateAsync(id, update);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await servicosService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);

    }

    [HttpGet("servico/{nome}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<ServicosResponse>> GetByNome([FromRoute] string nome)
    {
        var result = await servicosService.GetByNome(nome);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
}
