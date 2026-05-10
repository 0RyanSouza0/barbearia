namespace Barbearia.Application.Dtos.Endereco;

public record EnderecoResponse
{
    public int Id { get; init; }
    public string Cep { get; init; }
    public string Logradouro { get; init; }
    public DateTime CriadoEm { get; init; }
    public DateTime AtualizadoEm { get; init; }
    public int Numero { get; init; }
    public string Bairro { get; init; }
    public string Cidade { get; init; }
    public string Estado { get; init; }
}
