using Barbearia.Domain.Enuns;

namespace Barbearia.Domain.Entities;

public class Agendamento : BaseEntidade
{
    public Agendamento(int clienteId, int servicoId, int barbeiroId, DateTime dataHora)
    {
        ClienteId = clienteId;
        ServicoId = servicoId;
        BarbeiroId = barbeiroId;
        DataHora = dataHora;
        Status = StatusAgendamento.Agendado;
    }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public int ServicoId { get; set; }
    public Servicos Servicos { get; set; }
    public int BarbeiroId { get; set; }
    public Barbeiro Barbeiro { get; set; }
    public DateTime DataHora { get; set; }
    public StatusAgendamento Status { get; set; }

}


