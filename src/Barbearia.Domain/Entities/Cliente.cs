namespace Barbearia.Domain.Entities;

public class Cliente : BaseEntidade
{
    public Cliente(string nome, string telefone, string email, string senha)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
        Senha = senha;
    }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public bool Ativo { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }

}
