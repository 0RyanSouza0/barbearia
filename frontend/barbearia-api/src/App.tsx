import { useEffect, useMemo, useState } from "react";
import "./App.css";
import Layout from "./components/Layout";
import Agendamentos from "./pages/Agendamentos";
import Barbeiros from "./pages/Barbeiros";
import Cadastro from "./pages/Cadastro";
import Clientes from "./pages/Clientes";
import Dashboard from "./pages/Dashboard";
import Login from "./pages/Login";
import Pedidos from "./pages/Pedidos";
import Produtos from "./pages/Produtos";
import { getStoredAuth, logout } from "./services/auth";

const routes = {
  login: Login,
  cadastro: Cadastro,
  dashboard: Dashboard,
  produtos: Produtos,
  barbeiros: Barbeiros,
  clientes: Clientes,
  agendamentos: Agendamentos,
  pedidos: Pedidos,
};

export type AppRoute = keyof typeof routes;

function getRouteFromHash(): AppRoute {
  const hash = window.location.hash.replace("#/", "") as AppRoute;
  return hash in routes ? hash : "login";
}

export default function App() {
  const [route, setRoute] = useState<AppRoute>(getRouteFromHash);
  const [authVersion, setAuthVersion] = useState(0);

  useEffect(() => {
    const onHashChange = () => setRoute(getRouteFromHash());
    window.addEventListener("hashchange", onHashChange);
    return () => window.removeEventListener("hashchange", onHashChange);
  }, []);

  const auth = useMemo(() => getStoredAuth(), [authVersion, route]);
  const Page = routes[route];

  function navigate(nextRoute: AppRoute) {
    window.location.hash = `/${nextRoute}`;
    setRoute(nextRoute);
  }

  function handleAuthChange() {
    setAuthVersion((value) => value + 1);
    navigate("dashboard");
  }

  function handleLogout() {
    logout();
    setAuthVersion((value) => value + 1);
    navigate("login");
  }

  return (
    <Layout activeRoute={route} auth={auth} onLogout={handleLogout} onNavigate={navigate}>
      <Page auth={auth} onAuthChange={handleAuthChange} onNavigate={navigate} />
    </Layout>
  );
}
