namespace Barbearia.Domain.Entities;

public class Produto : BaseEntidade
{
    public Produto(string nome, decimal valor, int estoque, string descricao, int categoriaId)
    {
        Nome = nome;
        Valor = valor;
        Estoque = estoque;
        Descricao = descricao;
        Ativo = true;
        CategoriaId = categoriaId;
    }

    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public int Estoque { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
}
