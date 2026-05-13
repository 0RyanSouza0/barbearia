import type { PageProps } from "../types/api";

const cards = [
  { title: "Produtos", route: "produtos" as const, text: "Catálogo público e CRUD administrativo." },
  { title: "Barbeiros", route: "barbeiros" as const, text: "Equipe, especialidades e manutenção." },
  { title: "Clientes", route: "clientes" as const, text: "Cadastro e gestão de clientes." },
  { title: "Agendamentos", route: "agendamentos" as const, text: "Agenda com cliente, barbeiro e serviço." },
  { title: "Pedidos", route: "pedidos" as const, text: "Pedidos, itens e quantidades." },
];

export default function Dashboard({ auth, onNavigate }: PageProps) {
  return (
    <section className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Painel</p>
          <h1>Dashboard</h1>
          <p>{auth ? `Sessão ativa como ${auth.role}.` : "Faça login para acessar as áreas protegidas."}</p>
        </div>
      </header>

      <div className="dashboard-grid">
        {cards.map((card) => (
          <button className="metric-card" key={card.route} type="button" onClick={() => onNavigate(card.route)}>
            <span>{card.title}</span>
            <strong>{card.text}</strong>
          </button>
        ))}
      </div>
    </section>
  );
}
