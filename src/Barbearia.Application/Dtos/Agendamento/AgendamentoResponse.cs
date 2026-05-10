using Barbearia.Domain.Enuns;

namespace Barbearia.Application.Dtos.Agendamento;

public record AgendamentoResponse
{
    public int Id { get; init; }
    public int ClienteId { get; init; }
    public string NomeCliente { get; init; }
    public int ServicosId { get; init; }
    public string NomeServico { get; init; }
    public int BarbeiroId { get; init; }
    public string BarbeiroNome { get; init; }
    public StatusAgendamento StatusAgendamento { get; init; }
    public DateTime DataHora { get; init; }

}
