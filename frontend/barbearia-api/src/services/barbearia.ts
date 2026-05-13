import type { Agendamento, Barbeiro, Cliente, Pedido, Produto, Servico } from "../types/api";
import { api } from "./api";

export const produtoService = {
  list: () => api.get<Produto[]>("/api/Produto").then((response) => response.data),
  create: (payload: Omit<Produto, "id">) => api.post<Produto>("/api/Produto", payload).then((response) => response.data),
  update: (id: number, payload: Partial<Omit<Produto, "id">>) =>
    api.patch<Produto>(`/api/Produto/${id}`, payload).then((response) => response.data),
  remove: (id: number) => api.delete(`/api/Produto/${id}`).then((response) => response.data),
  categorias: () => api.get<string[]>("/api/Produto/categorias").then((response) => response.data),
};

export const clienteService = {
  list: () => api.get<Cliente[]>("/api/Cliente").then((response) => response.data),
  create: (payload: { nome: string; telefone: string; email: string; senha: string }) =>
    api.post<Cliente>("/api/Cliente", payload).then((response) => response.data),
  update: (id: number, payload: Partial<{ nome: string; telefone: string; email: string; senha: string }>) =>
    api.patch<Cliente>(`/api/Cliente/${id}`, payload).then((response) => response.data),
  remove: (id: number) => api.delete(`/api/Cliente/${id}`).then((response) => response.data),
};

export const barbeiroService = {
  list: () => api.get<Barbeiro[]>("/api/Barbeiro").then((response) => response.data),
  create: (payload: Omit<Barbeiro, "id">) =>
    api.post<Barbeiro>("/api/Barbeiro", payload).then((response) => response.data),
  update: (id: number, payload: Partial<Omit<Barbeiro, "id">>) =>
    api.patch<Barbeiro>(`/api/Barbeiro/${id}`, payload).then((response) => response.data),
  remove: (id: number) => api.delete(`/${id}`).then((response) => response.data),
};

export const servicoService = {
  list: () => api.get<Servico[]>("/api/Servicos").then((response) => response.data),
};

export const agendamentoService = {
  list: () => api.get<Agendamento[]>("/api/Agendamento").then((response) => response.data),
  create: (payload: { clienteId: number; servicosId: number; barbeiroId: number; dataHora: string }) =>
    api.post<Agendamento>("/api/Agendamento", payload).then((response) => response.data),
  update: (id: number, payload: { dataHora?: string }) =>
    api.patch<Agendamento>(`/api/Agendamento/${id}`, payload).then((response) => response.data),
  remove: (id: number) => api.delete(`/api/Agendamento/${id}`).then((response) => response.data),
  status: () => api.get<string[]>("/api/Agendamento/status-agendamento").then((response) => response.data),
};

export const pedidoService = {
  list: () => api.get<Pedido[]>("/api/Pedido").then((response) => response.data),
  create: (payload: { clienteId: number; itens: Array<{ produtoId: number; quantidade: number }> }) =>
    api.post<Pedido>("/api/Pedido", payload).then((response) => response.data),
  remove: (id: number) => api.delete(`/api/Pedido/${id}`).then((response) => response.data),
  addItem: (id: number, payload: { produtoId: number; quantidade: number }) =>
    api.patch<Pedido>(`/api/Pedido/adicionar-item/${id}`, payload).then((response) => response.data),
  updateItemQuantity: (id: number, payload: { produtoId: number; quantidade: number }) =>
    api.put<Pedido>(`/api/Pedido/atualizar-quantidade-item-pedido/${id}`, payload).then((response) => response.data),
  removeItem: (id: number, payload: { idProduto: number }) =>
    api.delete(`/api/Pedido/deletar-item-pedido/${id}`, { data: payload }).then((response) => response.data),
};
