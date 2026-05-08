using Barbearia.Application.Dtos.Produto;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController(IProdutoService produtoService) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<ProdutoResponse>> Create([FromBody] ProdutoRequest request)
    {
        var result = await produtoService.CreateAsync(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoResponse>> GetById([FromRoute] int id)
    {
        var result = await produtoService.GetByIdAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<ProdutoResponse>> Update([FromRoute] int id, [FromBody] ProdutoUpdate update)
    {
        var result = await produtoService.UpdateAsync(id, update);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return StatusCode(201, result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<ProdutoResponse>> GetAll()
    {
        var result = await produtoService.GetAllAsync();
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete([FromRoute] int id)
    {
        var result = await produtoService.DeleteAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("{nome}")]
    public async Task<ActionResult<ProdutoResponse>> GetByNome([FromRoute] string nome)
    {
        var result = await produtoService.GetProdutoByNome(nome);
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
    {
        var result = Enum.GetNames(typeof(Categoria));
        return Ok(result);
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IList<ProdutoResponse>>> GetProdutosByCategoria([FromQuery] Categoria? categoria)
    {
        var result = await produtoService.GetProdutosByCategoria(categoria.Value);
        return Ok(result);
    }

}
