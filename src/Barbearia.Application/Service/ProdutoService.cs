using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Produto;
using Barbearia.Application.Interfaces.Repository;
using Barbearia.Application.Interfaces.Service;
using Barbearia.Domain.Entities;
using FluentValidation;

namespace Barbearia.Application.Service
{
    public class ProdutoService(IProdutoRepository _produtoRepository, IValidator<ProdutoRequest> _validatorRequest
    , IValidator<ProdutoUpdate> _validatorUpdate) : IProdutoService
    {
        async Task<Result<ProdutoResponse>> IProdutoService.CreateAsync(ProdutoRequest request)
        {
            var resultValidator = await _validatorRequest.ValidateAsync(request);
            if (!resultValidator.IsValid)
            {
                return Result<ProdutoResponse>.Failure(resultValidator.Errors.Select(e => e.ErrorMessage).ToList());
            }
            var produto = new Produto
            (
                request.Nome,
                request.Valor,
                request.Estoque,
                request.Descricao,
                request.Categoria
            );

            produto.CriadoEm = DateTime.UtcNow;
            await _produtoRepository.Add(produto);
            await _produtoRepository.SaveChangesAsync();

            return Result<ProdutoResponse>.Success(new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Valor = produto.Valor,
                Estoque = produto.Estoque,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria
            });
        }

        async Task<Result<string>> IProdutoService.DeleteAsync(int id)
        {
            var produto = await _produtoRepository.GetById(id);
            if (produto is null)
            {
                return Result<string>.Failure("Produto não encontrado");
            }
            produto.Ativo = false;
            await _produtoRepository.Update(produto);
            await _produtoRepository.SaveChangesAsync();
            return Result<string>.Success("Produto deletado com sucesso");
        }

        async Task<Result<IEnumerable<ProdutoResponse>>> IProdutoService.GetAllAsync()
        {
            var produtos = await _produtoRepository.GetAll();
            return Result<IEnumerable<ProdutoResponse>>.Success(produtos.Select(p => new ProdutoResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Valor = p.Valor,
                Estoque = p.Estoque,
                Descricao = p.Descricao,
                Categoria = p.Categoria
            }));
        }

        async Task<Result<ProdutoResponse>> IProdutoService.GetByIdAsync(int id)
        {
            var produto = await _produtoRepository.GetById(id);
            if (produto is null)
            {
                return Result<ProdutoResponse>.Failure("Produto não encontrado");
            }
            return Result<ProdutoResponse>.Success(new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Valor = produto.Valor,
                Estoque = produto.Estoque,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria
            });
        }

        async Task<Result<IList<ProdutoResponse>>> IProdutoService.GetProdutosByCategoria(Categoria? categoria)
        {
            var categorias = await _produtoRepository.GetProdutoByCategoria(categoria.Value);
            return Result<IList<ProdutoResponse>>.Success(categorias.Select(p => new ProdutoResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Valor = p.Valor,
                Estoque = p.Estoque,
                Descricao = p.Descricao,
                Categoria = p.Categoria
            }).ToList());
        }

        async Task<Result<ProdutoResponse>> IProdutoService.GetProdutoByNome(string nome)
        {
            var produto = await _produtoRepository.GetProdutoByNameAsync(nome);
            if (produto is null)
            {
                return Result<ProdutoResponse>.Failure("Produto não encontrado");
            }
            return Result<ProdutoResponse>.Success(new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Valor = produto.Valor,
                Estoque = produto.Estoque,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria
            });
        }

        async Task<Result<ProdutoResponse>> IProdutoService.UpdateAsync(int id, ProdutoUpdate update)
        {
            var resultValidator = await _validatorUpdate.ValidateAsync(update);
            if (!resultValidator.IsValid)
            {
                var erros = resultValidator.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<ProdutoResponse>.Failure(erros);
            }
            var produto = await _produtoRepository.GetById(id);
            if (produto is null)
            {
                return Result<ProdutoResponse>.Failure("Produto não encontrado");
            }
            if (!string.IsNullOrWhiteSpace(update.Nome))
            {
                produto.Nome = update.Nome;
            }
            if (!string.IsNullOrWhiteSpace(update.Descricao))
            {
                produto.Descricao = update.Descricao;
            }
            if (update.Estoque.HasValue)
            {
                produto.Estoque = update.Estoque.Value;
            }
            if (update.Categoria.HasValue)
            {
                produto.Categoria = update.Categoria.Value;
            }
            if (update.Valor.HasValue)
            {
                produto.Valor = update.Valor.Value;
            }
            produto.AtualizadoEm = DateTime.UtcNow;
            await _produtoRepository.Update(produto);
            await _produtoRepository.SaveChangesAsync();

            return Result<ProdutoResponse>.Success(new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Valor = produto.Valor,
                Estoque = produto.Estoque,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria
            });
        }
    }
}