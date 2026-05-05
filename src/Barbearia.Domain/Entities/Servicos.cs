namespace Barbearia.Domain.Entities;

public class Servicos : BaseEntidade
{
    public Servicos(string nome, string descricao, decimal valor, int duracaoMinutos)
    {
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
        DuracaoMinutos = duracaoMinutos;

    }

    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public int DuracaoMinutos { get; set; }
}

