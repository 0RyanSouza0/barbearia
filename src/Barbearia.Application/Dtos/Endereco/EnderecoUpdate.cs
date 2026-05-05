namespace Barbearia.Application.Dtos.Endereco;

public record EnderecoUpdate
{
    public string? Logradouro { get; set; }
    public string? Cep { get; set; }
    public string? Bairro { get; set; }
    public int? Numero { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
}
