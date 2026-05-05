using Barbearia.Application.Comom;
using Barbearia.Application.Dtos.Endereco;

namespace Barbearia.Application.Interfaces.Service;

public interface IEnderecoService
{
    Task<Result<EnderecoResponse>> CreateAsync(EnderecoRequest enderecoRequest);
    Task<Result<EnderecoResponse>> UpdateAsync(int id, EnderecoRequest enderecoRequest);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<EnderecoResponse>> GetByIdAsync(int id);
    Task<Result<IEnumerable<EnderecoResponse>>> GetAllAsync();

}
