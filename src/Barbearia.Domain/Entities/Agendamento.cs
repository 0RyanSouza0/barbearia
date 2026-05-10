using Barbearia.Domain.Enuns;

namespace Barbearia.Domain.Entities;

public class Agendamento : BaseEntidade
{
    public Agendamento(int clienteId, int servicosId, int barbeiroId, DateTime dataHora)
    {
        ClienteId = clienteId;
        ServicosId = servicosId;
        BarbeiroId = barbeiroId;
        DataHora = dataHora;
        Status = StatusAgendamento.Agendado;
    }
    public int ClienteId { get; private set; }
    public Cliente Cliente { get; private set; }
    public int ServicosId { get; private set; }
    public Servicos Servicos { get; private set; }
    public int BarbeiroId { get; private set; }
    public Barbeiro Barbeiro { get; private set; }
    public DateTime DataHora { get; set; }
    public StatusAgendamento Status { get; private set; }

    public void ParaAtualizarAgendamento()
    {
        if (Status == StatusAgendamento.Concluido)
        {
            throw new InvalidOperationException("Agendamento concluido nao pode ser atualizado");
        }
        if (Status == StatusAgendamento.Cancelado)
        {
            throw new InvalidOperationException("Agendamento cancelado nao pode ser atualizado");
        }
    }
}


