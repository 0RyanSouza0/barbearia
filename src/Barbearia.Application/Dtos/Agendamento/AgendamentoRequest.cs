namespace Barbearia.Application.Dtos.Agendamento;

public record AgendamentoRequest
{

    public int ClienteId { get; set; }
    public int ServicosId { get; set; }
    public int BarbeiroId { get; set; }
    public DateTime DataHora { get; set; }
}
