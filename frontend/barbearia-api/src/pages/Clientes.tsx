import ResourcePage from "../components/ResourcePage";
import { clienteService } from "../services/barbearia";
import type { Cliente, FieldConfig, PageProps } from "../types/api";

type ClienteForm = {
  nome: string;
  telefone: string;
  email: string;
  senha: string;
};

const fields: FieldConfig<ClienteForm>[] = [
  { name: "nome", label: "Nome", required: true },
  { name: "telefone", label: "Telefone", required: true },
  { name: "email", label: "Email", type: "email", required: true },
  { name: "senha", label: "Senha", type: "password", required: true },
];

export default function Clientes({ auth }: PageProps) {
  const isAdmin = auth?.role === "Admin";

  return (
    <ResourcePage<Cliente, ClienteForm>
      title="Clientes"
      subtitle="Área administrativa para consultar e manter clientes."
      fields={fields}
      initialForm={{ nome: "", telefone: "", email: "", senha: "" }}
      columns={[
        { key: "id", label: "ID" },
        { key: "nome", label: "Nome" },
        { key: "telefone", label: "Telefone" },
        { key: "email", label: "Email" },
      ]}
      list={clienteService.list}
      create={isAdmin ? clienteService.create : undefined}
      update={isAdmin ? clienteService.update : undefined}
      remove={isAdmin ? clienteService.remove : undefined}
      readOnly={!isAdmin}
    />
  );
}
