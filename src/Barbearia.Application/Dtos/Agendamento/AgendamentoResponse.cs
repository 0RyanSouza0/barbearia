namespace Barbearia.Application.Dtos.Agendamento;

public record AgendamentoResponse
{
    public int Id { get; init; }
    public int ClienteId { get; init; }
    public int ServicoId { get; init; }
    public int BarbeiroId { get; init; }
    public DateTime DataHora { get; init; }

}
