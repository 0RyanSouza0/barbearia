import ResourcePage from "../components/ResourcePage";
import { produtoService } from "../services/barbearia";
import type { FieldConfig, PageProps, Produto } from "../types/api";

type ProdutoForm = {
  nome: string;
  valor: number;
  estoque: number;
  descricao: string;
  categoria: string;
};

const categorias = ["Pomada", "Gel", "Shampoo", "Condicionador", "OleoBarba", "Navalha", "MaquinaCorte", "Acessorios"];

const fields: FieldConfig<ProdutoForm>[] = [
  { name: "nome", label: "Nome", required: true },
  { name: "valor", label: "Valor", type: "number", required: true, min: 0 },
  { name: "estoque", label: "Estoque", type: "number", required: true, min: 0 },
  { name: "categoria", label: "Categoria", type: "select", required: true, options: categorias },
  { name: "descricao", label: "Descrição", type: "textarea", required: true },
];

export default function Produtos({ auth }: PageProps) {
  const isAdmin = auth?.role === "Admin";

  return (
    <ResourcePage<Produto, ProdutoForm>
      title="Produtos"
      subtitle="Lista pública, criação e manutenção para administradores."
      fields={fields}
      initialForm={{ nome: "", valor: 0, estoque: 0, descricao: "", categoria: categorias[0] }}
      columns={[
        { key: "id", label: "ID" },
        { key: "nome", label: "Nome" },
        { key: "categoria", label: "Categoria" },
        { key: "estoque", label: "Estoque" },
        { key: "valor", label: "Valor", render: (item) => `R$ ${Number(item.valor).toFixed(2)}` },
      ]}
      list={produtoService.list}
      create={isAdmin ? (payload) => produtoService.create(payload) : undefined}
      update={isAdmin ? (id, payload) => produtoService.update(id, payload) : undefined}
      remove={isAdmin ? produtoService.remove : undefined}
      readOnly={!isAdmin}
    />
  );
}
