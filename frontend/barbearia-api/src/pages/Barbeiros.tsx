import ResourcePage from "../components/ResourcePage";
import { barbeiroService } from "../services/barbearia";
import type { Barbeiro, FieldConfig, PageProps } from "../types/api";

type BarbeiroForm = {
  nome: string;
  especialidade: string;
};

const fields: FieldConfig<BarbeiroForm>[] = [
  { name: "nome", label: "Nome", required: true },
  { name: "especialidade", label: "Especialidade", required: true },
];

export default function Barbeiros({ auth }: PageProps) {
  const isAdmin = auth?.role === "Admin";

  return (
    <ResourcePage<Barbeiro, BarbeiroForm>
      title="Barbeiros"
      subtitle="Consome as rotas de listagem, criação, edição e exclusão existentes."
      fields={fields}
      initialForm={{ nome: "", especialidade: "" }}
      columns={[
        { key: "id", label: "ID" },
        { key: "nome", label: "Nome" },
        { key: "especialidade", label: "Especialidade" },
      ]}
      list={barbeiroService.list}
      create={isAdmin ? barbeiroService.create : undefined}
      update={isAdmin ? barbeiroService.update : undefined}
      remove={isAdmin ? barbeiroService.remove : undefined}
      readOnly={!isAdmin}
    />
  );
}
