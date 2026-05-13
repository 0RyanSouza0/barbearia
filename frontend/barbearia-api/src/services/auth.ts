import type { AuthSession, Role } from "../types/api";
import { api } from "./api";

const TOKEN_KEY = "barbearia.token";
const ROLE_KEY = "barbearia.role";

type LoginPayload = {
  email: string;
  senha: string;
};

export async function login(role: Role, payload: LoginPayload) {
  const endpoint = role === "Admin" ? "/api/login/admin" : "/api/login/cliente";
  const response = await api.post<string>(endpoint, payload);
  const token = response.data;

  localStorage.setItem(TOKEN_KEY, token);
  localStorage.setItem(ROLE_KEY, role);

  return { token, role };
}

export async function cadastrarCliente(
  payload: LoginPayload & { nome: string; telefone: string },
) {
  const response = await api.post("/api/cliente", payload);
  return response.data;
}

export async function cadastrarAdmin(payload: LoginPayload & { nome: string }) {
  const response = await api.post("/api/administrador", payload);
  return response.data;
}

export function getStoredAuth(): AuthSession | null {
  const token = localStorage.getItem(TOKEN_KEY);
  const storedRole = localStorage.getItem(ROLE_KEY);
  const role =
    storedRole === "Admin" || storedRole === "Cliente" ? storedRole : null;

  return token && role ? { token, role } : null;
}

export function logout() {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(ROLE_KEY);
}
