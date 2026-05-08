namespace Barbearia.Domain.Entities;

public class Produto : BaseEntidade
{
    public Produto(string nome, decimal valor, int estoque, string descricao, Categoria categoria)
    {
        Nome = nome;
        Valor = valor;
        Estoque = estoque;
        Descricao = descricao;
        Ativo = true;
        Categoria = categoria;

    }

    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public int Estoque { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }
    public Categoria Categoria { get; set; }

    public void AumentarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.");
        if (quantidade > Estoque)
            throw new ArgumentException("Quantidade não pode ser maior que o estoque.");
        Estoque += quantidade;
    }
}
