import { useState } from "react";
import { login } from "../services/auth";
import type { PageProps, Role } from "../types/api";

export default function Login({ onAuthChange, onNavigate }: PageProps) {
  const [role, setRole] = useState<Role>("Admin");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleLogin(event: React.FormEvent) {
    event.preventDefault();
    setLoading(true);
    setError("");

    try {
      await login(role, { email, senha });
      onAuthChange();
    } catch {
      setError("Email ou senha inválidos para o perfil selecionado.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="auth-page">
      <div className="auth-card">
        <p className="eyebrow">Acesso JWT</p>
        <h1>Entrar</h1>
        <p>Use uma conta Admin ou Cliente cadastrada na API.</p>

        {error && <p className="alert">{error}</p>}

        <form className="form-stack" onSubmit={handleLogin}>
          <label className="field">
            <span>Perfil</span>
            <select value={role} onChange={(event) => setRole(event.target.value as Role)}>
              <option value="Admin">Admin</option>
              <option value="Cliente">Cliente</option>
            </select>
          </label>

          <label className="field">
            <span>Email</span>
            <input required type="email" value={email} onChange={(event) => setEmail(event.target.value)} />
          </label>

          <label className="field">
            <span>Senha</span>
            <input required type="password" value={senha} onChange={(event) => setSenha(event.target.value)} />
          </label>

          <button className="primary-button" disabled={loading} type="submit">
            {loading ? "Entrando..." : "Entrar"}
          </button>
        </form>

        <button className="link-button" type="button" onClick={() => onNavigate("cadastro")}>
          Criar uma conta
        </button>
      </div>
    </section>
  );
}
