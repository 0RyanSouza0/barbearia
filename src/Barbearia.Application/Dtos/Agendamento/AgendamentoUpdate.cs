namespace Barbearia.Application.Dtos.Agendamento;

public class AgendamentoUpdate
{
    public int? ClienteId { get; set; }
    public int? ServicoId { get; set; }
    public int? BarbeiroId { get; set; }
    public DateTime? DataHora { get; set; }
    public string? Status { get; set; }
}
