namespace Barbearia.Application.Dtos.Agendamento;

public class AgendamentoRequest
{

    public int ClienteId { get; set; }
    public int ServicoId { get; set; }
    public int BarbeiroId { get; set; }
    public DateTime DataHora { get; set; }
}
