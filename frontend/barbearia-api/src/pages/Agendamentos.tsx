import { useEffect, useState } from "react";
import {
  agendamentoService,
  barbeiroService,
  clienteService,
  servicoService,
} from "../services/barbearia";
import type {
  Agendamento,
  Barbeiro,
  Cliente,
  PageProps,
  Servico,
} from "../types/api";

type AgendamentoForm = {
  clienteId: number;
  servicosId: number;
  barbeiroId: number;
  dataHora: string;
};

const emptyForm: AgendamentoForm = {
  clienteId: 0,
  servicosId: 0,
  barbeiroId: 0,
  dataHora: "",
};

export default function Agendamentos({ auth }: PageProps) {
  const [items, setItems] = useState<Agendamento[]>([]);
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [barbeiros, setBarbeiros] = useState<Barbeiro[]>([]);
  const [servicos, setServicos] = useState<Servico[]>([]);
  const [form, setForm] = useState(emptyForm);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [error, setError] = useState("");
  const isAdmin = auth?.role === "Admin";

  async function load() {
    setError("");
    try {
      const [agendamentos, clientesData, barbeirosData, servicosData] =
        await Promise.all([
          agendamentoService.list(),
          isAdmin ? clienteService.list() : Promise.resolve([]),
          barbeiroService.list(),
          servicoService.list(),
        ]);
      setItems(agendamentos);
      setClientes(clientesData);
      setBarbeiros(barbeirosData);
      setServicos(servicosData);
    } catch {
      setError("Não foi possível carregar agendamentos ou dados auxiliares.");
    }
  }

  useEffect(() => {
    if (auth) load();
  }, [auth?.role]);

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    setError("");

    try {
      if (editingId) {
        await agendamentoService.update(editingId, { dataHora: form.dataHora });
      } else {
        await agendamentoService.create(form);
      }
      setForm(emptyForm);
      setEditingId(null);
      await load();
    } catch {
      setError("Não foi possível salvar o agendamento.");
    }
  }

  async function remove(id: number) {
    if (!window.confirm("Confirmar exclusão?")) return;
    await agendamentoService.remove(id);
    await load();
  }

  if (!auth)
    return <p className="alert">Faça login para acessar agendamentos.</p>;

  return (
    <section className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Agenda</p>
          <h1>Agendamentos</h1>
          <p>Usa clientes, barbeiros e serviços já expostos pela API.</p>
        </div>
        <button className="ghost-button" type="button" onClick={load}>
          Atualizar
        </button>
      </header>

      {error && <p className="alert">{error}</p>}

      {isAdmin && (
        <form className="panel form-grid" onSubmit={handleSubmit}>
          {!editingId && (
            <>
              <label className="field">
                <span>Cliente</span>
                <select
                  required
                  value={form.clienteId}
                  onChange={(event) =>
                    setForm({ ...form, clienteId: Number(event.target.value) })
                  }
                >
                  <option value={0}>Selecione</option>
                  {clientes.map((cliente) => (
                    <option key={cliente.id} value={cliente.id}>
                      {cliente.nome}
                    </option>
                  ))}
                </select>
              </label>

              <label className="field">
                <span>Serviço</span>
                <select
                  required
                  value={form.servicosId}
                  onChange={(event) =>
                    setForm({ ...form, servicosId: Number(event.target.value) })
                  }
                >
                  <option value={0}>Selecione</option>
                  {servicos.map((servico) => (
                    <option key={servico.id} value={servico.id}>
                      {servico.nome}
                    </option>
                  ))}
                </select>
              </label>

              <label className="field">
                <span>Barbeiro</span>
                <select
                  required
                  value={form.barbeiroId}
                  onChange={(event) =>
                    setForm({ ...form, barbeiroId: Number(event.target.value) })
                  }
                >
                  <option value={0}>Selecione</option>
                  {barbeiros.map((barbeiro) => (
                    <option key={barbeiro.id} value={barbeiro.id}>
                      {barbeiro.nome}
                    </option>
                  ))}
                </select>
              </label>
            </>
          )}

          <label className="field">
            <span>Data e hora</span>
            <input
              required
              type="datetime-local"
              value={form.dataHora}
              onChange={(event) =>
                setForm({ ...form, dataHora: event.target.value })
              }
            />
          </label>

          <div className="form-actions wide">
            <button className="primary-button" type="submit">
              {editingId ? "Salvar alteração" : "Criar"}
            </button>
            {editingId && (
              <button
                className="ghost-button"
                type="button"
                onClick={() => {
                  setEditingId(null);
                  setForm(emptyForm);
                }}
              >
                Cancelar
              </button>
            )}
          </div>
        </form>
      )}

      <div className="panel table-wrap">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Cliente</th>
              <th>Serviço</th>
              <th>Barbeiro</th>
              <th>Status</th>
              <th>Data</th>
              {isAdmin && <th>Ações</th>}
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.nomeCliente}</td>
                <td>{item.nomeServico}</td>
                <td>{item.barbeiroNome}</td>
                <td>{String(item.statusAgendamento)}</td>
                <td>{new Date(item.dataHora).toLocaleString("pt-BR")}</td>
                {isAdmin && (
                  <td className="row-actions">
                    <button
                      className="ghost-button compact"
                      type="button"
                      onClick={() => {
                        setEditingId(item.id);
                        setForm({
                          clienteId: item.clienteId,
                          servicosId: item.servicosId,
                          barbeiroId: item.barbeiroId,
                          dataHora: item.dataHora.slice(0, 16),
                        });
                      }}
                    >
                      Editar
                    </button>
                    <button
                      className="danger-button compact"
                      type="button"
                      onClick={() => remove(item.id)}
                    >
                      Excluir
                    </button>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}
