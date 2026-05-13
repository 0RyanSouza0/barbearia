import { useState } from "react";
import { cadastrarAdmin, cadastrarCliente } from "../services/auth";
import type { PageProps, Role } from "../types/api";

export default function Cadastro({ onNavigate }: PageProps) {
  const [role, setRole] = useState<Role>("Cliente");
  const [nome, setNome] = useState("");
  const [telefone, setTelefone] = useState("");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleCadastro(event: React.FormEvent) {
    event.preventDefault();
    setLoading(true);
    setMessage("");
    setError("");

    try {
      12345678;
      if (role === "Admin") {
        await cadastrarAdmin({ nome, email, senha });
      } else {
        await cadastrarCliente({ nome, telefone, email, senha });
      }
      setMessage("Cadastro realizado. Você já pode fazer login.");
      setNome("");
      setTelefone("");
      setEmail("");
      setSenha("");
    } catch {
      setError("Não foi possível cadastrar com os dados informados.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="auth-page">
      <div className="auth-card">
        <p className="eyebrow">Cadastro</p>
        <h1>Criar conta</h1>
        <p>
          Rotas usadas: Cliente e Administrador, conforme disponíveis no
          backend.
        </p>

        {message && <p className="success">{message}</p>}
        {error && <p className="alert">{error}</p>}

        <form className="form-stack" onSubmit={handleCadastro}>
          <label className="field">
            <span>Tipo de conta</span>
            <select
              value={role}
              onChange={(event) => setRole(event.target.value as Role)}
            >
              <option value="Cliente">Cliente</option>
              <option value="Admin">Admin</option>
            </select>
          </label>

          <label className="field">
            <span>Nome</span>
            <input
              required
              value={nome}
              onChange={(event) => setNome(event.target.value)}
            />
          </label>

          {role === "Cliente" && (
            <label className="field">
              <span>Telefone</span>
              <input
                required
                value={telefone}
                onChange={(event) => setTelefone(event.target.value)}
              />
            </label>
          )}

          <label className="field">
            <span>Email</span>
            <input
              required
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
            />
          </label>

          <label className="field">
            <span>Senha</span>
            <input
              required
              type="password"
              value={senha}
              onChange={(event) => setSenha(event.target.value)}
            />
          </label>

          <button className="primary-button" disabled={loading} type="submit">
            {loading ? "Cadastrando..." : "Cadastrar"}
          </button>
        </form>

        <button
          className="link-button"
          type="button"
          onClick={() => onNavigate("login")}
        >
          Voltar para login
        </button>
      </div>
    </section>
  );
}
