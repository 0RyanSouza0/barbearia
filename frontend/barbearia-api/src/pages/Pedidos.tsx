import { useEffect, useState } from "react";
import {
  clienteService,
  pedidoService,
  produtoService,
} from "../services/barbearia";
import type { Cliente, PageProps, Pedido, Produto } from "../types/api";

type PedidoForm = {
  clienteId: number;
  produtoId: number;
  quantidade: number;
};

const emptyForm: PedidoForm = {
  clienteId: 0,
  produtoId: 0,
  quantidade: 1,
};

export default function Pedidos({ auth }: PageProps) {
  const [pedidos, setPedidos] = useState<Pedido[]>([]);
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [form, setForm] = useState(emptyForm);
  const [itemForm, setItemForm] = useState({
    pedidoId: 0,
    produtoId: 0,
    quantidade: 1,
  });
  const [error, setError] = useState("");
  const isAdmin = auth?.role === "Admin";

  async function load() {
    setError("");
    try {
      const [pedidosData, produtosData, clientesData] = await Promise.all([
        pedidoService.list(),
        produtoService.list(),
        isAdmin ? clienteService.list() : Promise.resolve([]),
      ]);
      setPedidos(pedidosData);
      setProdutos(produtosData);
      setClientes(clientesData);
    } catch {
      setError("Não foi possível carregar pedidos.");
    }
  }

  useEffect(() => {
    if (auth) load();
  }, [auth?.role]);

  async function handleCreate(event: React.FormEvent) {
    event.preventDefault();
    setError("");

    try {
      await pedidoService.create({
        clienteId: form.clienteId,
        itens: [{ produtoId: form.produtoId, quantidade: form.quantidade }],
      });
      setForm(emptyForm);
      await load();
    } catch {
      setError("Não foi possível criar o pedido.");
    }
  }

  async function handleItemAction(action: "add" | "update" | "remove") {
    setError("");
    try {
      if (action === "add") {
        await pedidoService.addItem(itemForm.pedidoId, {
          produtoId: itemForm.produtoId,
          quantidade: itemForm.quantidade,
        });
      }
      if (action === "update") {
        await pedidoService.updateItemQuantity(itemForm.pedidoId, {
          produtoId: itemForm.produtoId,
          quantidade: itemForm.quantidade,
        });
      }
      if (action === "remove") {
        await pedidoService.removeItem(itemForm.pedidoId, {
          idProduto: itemForm.produtoId,
        });
      }
      setItemForm({ pedidoId: 0, produtoId: 0, quantidade: 1 });
      await load();
    } catch {
      setError("Não foi possível alterar os itens do pedido.");
    }
  }

  async function removePedido(id: number) {
    if (!window.confirm("Confirmar exclusão?")) return;
    await pedidoService.remove(id);
    await load();
  }

  if (!auth) return <p className="alert">Faça login para acessar pedidos.</p>;

  return (
    <section className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Vendas</p>
          <h1>Pedidos</h1>
          <p>
            Criação de pedido com itens e rotas existentes para manutenção dos
            itens.
          </p>
        </div>
        <button className="ghost-button" type="button" onClick={load}>
          Atualizar
        </button>
      </header>

      {error && <p className="alert">{error}</p>}

      <form className="panel form-grid" onSubmit={handleCreate}>
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
          <span>Produto</span>
          <select
            required
            value={form.produtoId}
            onChange={(event) =>
              setForm({ ...form, produtoId: Number(event.target.value) })
            }
          >
            <option value={0}>Selecione</option>
            {produtos.map((produto) => (
              <option key={produto.id} value={produto.id}>
                {produto.nome}
              </option>
            ))}
          </select>
        </label>

        <label className="field">
          <span>Quantidade</span>
          <input
            min={1}
            required
            type="number"
            value={form.quantidade}
            onChange={(event) =>
              setForm({ ...form, quantidade: Number(event.target.value) })
            }
          />
        </label>

        <div className="form-actions wide">
          <button className="primary-button" type="submit">
            Criar pedido
          </button>
        </div>
      </form>

      <div className="panel form-grid">
        <label className="field">
          <span>Pedido</span>
          <select
            value={itemForm.pedidoId}
            onChange={(event) =>
              setItemForm({ ...itemForm, pedidoId: Number(event.target.value) })
            }
          >
            <option value={0}>Selecione</option>
            {pedidos.map((pedido) => (
              <option key={pedido.id} value={pedido.id}>
                Pedido #{pedido.id}
              </option>
            ))}
          </select>
        </label>

        <label className="field">
          <span>Produto</span>
          <select
            value={itemForm.produtoId}
            onChange={(event) =>
              setItemForm({
                ...itemForm,
                produtoId: Number(event.target.value),
              })
            }
          >
            <option value={0}>Selecione</option>
            {produtos.map((produto) => (
              <option key={produto.id} value={produto.id}>
                {produto.nome}
              </option>
            ))}
          </select>
        </label>

        <label className="field">
          <span>Quantidade</span>
          <input
            min={1}
            type="number"
            value={itemForm.quantidade}
            onChange={(event) =>
              setItemForm({
                ...itemForm,
                quantidade: Number(event.target.value),
              })
            }
          />
        </label>

        <div className="form-actions wide">
          <button
            className="ghost-button"
            type="button"
            onClick={() => handleItemAction("add")}
          >
            Adicionar item
          </button>
          <button
            className="ghost-button"
            type="button"
            onClick={() => handleItemAction("update")}
          >
            Atualizar quantidade
          </button>
          <button
            className="danger-button"
            type="button"
            onClick={() => handleItemAction("remove")}
          >
            Remover item
          </button>
        </div>
      </div>

      <div className="panel table-wrap">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Cliente</th>
              <th>Status</th>
              <th>Total</th>
              <th>Itens</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {pedidos.map((pedido) => (
              <tr key={pedido.id}>
                <td>{pedido.id}</td>
                <td>{pedido.idCliente}</td>
                <td>{String(pedido.status)}</td>
                <td>R$ {Number(pedido.valorTotal).toFixed(2)}</td>
                <td>
                  {(pedido.itemPedidos ?? []).map((item) => (
                    <span className="item-chip" key={item.id}>
                      Produto {item.produtoId} x{item.quantidade}
                    </span>
                  ))}
                </td>
                <td>
                  <button
                    className="danger-button compact"
                    type="button"
                    onClick={() => removePedido(pedido.id)}
                  >
                    Excluir
                  </button>
                </td>
              </tr>
            ))}
            {!pedidos.length && (
              <tr>
                <td colSpan={6}>Nenhum pedido encontrado.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
