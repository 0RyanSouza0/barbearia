using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Barbeiro;
using Barbearia.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BarbeiroController(IBarbeiroService barbeiroService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BarbeiroResponse>> Create([FromBody] BarbeiroRequest request)
    {
        var result = await barbeiroService.CreateAsync(request);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return StatusCode(201, result.Value);
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<BarbeiroResponse>> GetAll()
    {
        var result = await barbeiroService.GetAllAsync();

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }


    [HttpGet("listar/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BarbeiroResponse>> GetById([FromRoute] int id)
    {
        var result = await barbeiroService.GetByIdAsync(id);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }


    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BarbeiroResponse>> Update([FromRoute] int id, [FromBody] BarbeiroUpdate update)
    {
        var result = await barbeiroService.UpdateAsync(id, update);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return StatusCode(201, result.Value);
    }


    [HttpGet("{especialidade}")]
    [Authorize(Roles = "Admin,Cliente")]
    public async Task<ActionResult<BarbeiroResponse>> GetByEspecialidade([FromRoute] string especialidade)
    {
        var result = await barbeiroService.GetByEspecialidadeAsync(especialidade);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await barbeiroService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
}
