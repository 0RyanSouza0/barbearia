using Barbearia.Application.Dtos.Administrador;
using Barbearia.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdministradorController(IAdministradorService administradorService) : ControllerBase
{

    [HttpPost]

    public async Task<ActionResult<AdministradorResponse>> Create([FromBody] AdministradorRequest request)
    {
        var result = await administradorService.CreateAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdministradorResponse>> GetAll()
    {
        var result = await administradorService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("relatorio")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<AdministradorResponse>>> GetAllClientesRelatorio()
    {
        var result = await administradorService.GetAllRelatorioAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdministradorResponse>> GetById([FromRoute] int id)
    {
        var result = await administradorService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdministradorResponse>> GetByEmail([FromRoute] string email)
    {
        var result = await administradorService.GetByEmailAsync(email);
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
        var result = await administradorService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdministradorResponse>> Update([FromRoute] int id, [FromBody] AdministradorRequest update)
    {
        var result = await administradorService.UpdateAsync(id, update);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }
}
