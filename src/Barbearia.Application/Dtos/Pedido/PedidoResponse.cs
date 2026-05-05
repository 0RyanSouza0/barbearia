namespace Barbearia.Application.Dtos.Pedido;

public record PedidoResponse
{
    public int Id { get; init; }
    public DateTime Data { get; init; }
    public decimal ValorTotal { get; init; }
    public string Status { get; init; }
    public int IdBarbeiro { get; init; }
    public int IdCliente { get; init; }
}
