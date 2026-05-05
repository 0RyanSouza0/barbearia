namespace Barbearia.Application.Dtos.Produto;

public class ProdutoRequest
{
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public int Estoque { get; set; }
    public string Descricao { get; set; }
    public int CategoriaId { get; set; }

}
