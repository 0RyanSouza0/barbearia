namespace Barbearia.Domain.Entities;

public class Endereco : BaseEntidade
{
    public Endereco(string logradouro, string cep, string bairro, int numero, string cidade, string estado)
    {
        Logradouro = logradouro;
        Cep = cep;
        Bairro = bairro;
        Numero = numero;
        Cidade = cidade;
        Estado = estado;
    }
    public string Logradouro { get; set; }
    public string Cep { get; set; }
    public string Bairro { get; set; }
    public int Numero { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}
