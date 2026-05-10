using Barbearia.Domain.Enuns;

namespace Barbearia.Domain.Entities;

public class Administrador : BaseEntidade
{
    public Administrador(string nome, string email, string senha)
    {
        Nome = nome;
        Email = email;
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
        Senha = senha;
        Role = Role.Admin;
    }
    public bool Ativo { get; set; }
    public string Email { get; set; }
    public string Nome { get; set; }
    public Role Role { get; set; }
    public string Senha { get; set; }
}
