namespace Barbearia.Domain.Entities;

public class Barbeiro : BaseEntidade
{
    public Barbeiro(string nome, string especialidade)
    {

        Nome = nome;
        Especialidade = especialidade;
        Ativo = true;
    }
    public string Nome { get; set; }
    public string Especialidade { get; set; }
    public bool Ativo { get; set; }
}
