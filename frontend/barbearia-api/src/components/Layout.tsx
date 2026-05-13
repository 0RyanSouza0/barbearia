import type { PropsWithChildren } from "react";
import type { AppRoute } from "../App";
import type { AuthSession } from "../types/api";

type LayoutProps = PropsWithChildren<{
  activeRoute: AppRoute;
  auth: AuthSession | null;
  onLogout: () => void;
  onNavigate: (route: AppRoute) => void;
}>;

const navItems: Array<{ route: AppRoute; label: string; protected?: boolean }> = [
  { route: "dashboard", label: "Dashboard", protected: true },
  { route: "produtos", label: "Produtos" },
  { route: "barbeiros", label: "Barbeiros", protected: true },
  { route: "clientes", label: "Clientes", protected: true },
  { route: "agendamentos", label: "Agendamentos", protected: true },
  { route: "pedidos", label: "Pedidos", protected: true },
];

export default function Layout({ activeRoute, auth, children, onLogout, onNavigate }: LayoutProps) {
  return (
    <div className="app-shell">
      <aside className="sidebar">
        <button className="brand" type="button" onClick={() => onNavigate(auth ? "dashboard" : "login")}>
          <span className="brand-mark">B</span>
          <span>
            <strong>Barbearia</strong>
            <small>API Admin</small>
          </span>
        </button>

        <nav className="nav-list" aria-label="Navegação principal">
          {navItems.map((item) => {
            const disabled = item.protected && !auth;
            return (
              <button
                className={activeRoute === item.route ? "nav-item active" : "nav-item"}
                disabled={disabled}
                key={item.route}
                type="button"
                onClick={() => onNavigate(item.route)}
              >
                {item.label}
              </button>
            );
          })}
        </nav>

        <div className="auth-box">
          {auth ? (
            <>
              <span className="pill">{auth.role}</span>
              <button className="ghost-button" type="button" onClick={onLogout}>
                Sair
              </button>
            </>
          ) : (
            <>
              <button className="primary-button" type="button" onClick={() => onNavigate("login")}>
                Login
              </button>
              <button className="ghost-button" type="button" onClick={() => onNavigate("cadastro")}>
                Cadastro
              </button>
            </>
          )}
        </div>
      </aside>

      <main className="content">{children}</main>
    </div>
  );
}
