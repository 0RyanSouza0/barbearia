export type Role = "Admin" | "Cliente";

export type AuthSession = {
  token: string;
  role: Role;
};

export type PageProps = {
  auth: AuthSession | null;
  onAuthChange: () => void;
  onNavigate: (route: import("../App").AppRoute) => void;
};

export type Cliente = {
  id: number;
  criadoEm?: string;
  atualizadoEm?: string;
  nome: string;
  ativo?: boolean;
  telefone: string;
  email: string;
};

export type Produto = {
  id: number;
  nome: string;
  valor: number;
  estoque: number;
  descricao: string;
  dataCriacao?: string;
  dataAtualizacao?: string;
  ativo?: boolean;
  categoria: string | number;
};

export type Barbeiro = {
  id: number;
  nome: string;
  especialidade: string;
};

export type Servico = {
  id: number;
  nome: string;
  descricao: string;
  valor: number;
  duracaoMinutos: number;
};

export type Agendamento = {
  id: number;
  clienteId: number;
  nomeCliente: string;
  servicosId: number;
  nomeServico: string;
  barbeiroId: number;
  barbeiroNome: string;
  statusAgendamento: string | number;
  dataHora: string;
};

export type ItemPedido = {
  id: number;
  pedidoId: number;
  produtoId: number;
  quantidade: number;
  valorUnitario: number;
};

export type Pedido = {
  id: number;
  data: string;
  dataAtualizacao: string;
  valorTotal: number;
  status: string | number;
  itemPedidos: ItemPedido[];
  idCliente: number;
};

export type FieldType = "text" | "email" | "password" | "number" | "datetime-local" | "select" | "textarea";

export type FieldConfig<TForm extends Record<string, unknown>> = {
  name: keyof TForm;
  label: string;
  type?: FieldType;
  required?: boolean;
  options?: string[];
  min?: number;
};
