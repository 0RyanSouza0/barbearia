import { useEffect, useMemo, useState } from "react";
import type { FieldConfig } from "../types/api";

type ResourcePageProps<TItem extends { id: number }, TForm extends Record<string, unknown>> = {
  title: string;
  subtitle: string;
  fields: FieldConfig<TForm>[];
  initialForm: TForm;
  columns: Array<{ key: keyof TItem; label: string; render?: (item: TItem) => React.ReactNode }>;
  list: () => Promise<TItem[]>;
  create?: (payload: TForm) => Promise<unknown>;
  update?: (id: number, payload: Partial<TForm>) => Promise<unknown>;
  remove?: (id: number) => Promise<unknown>;
  readOnly?: boolean;
};

function normalizeError(error: unknown) {
  if (typeof error === "object" && error !== null && "response" in error) {
    const response = (error as { response?: { data?: unknown } }).response;
    if (typeof response?.data === "string") return response.data;
    if (Array.isArray(response?.data)) return response.data.join(", ");
  }

  return "Não foi possível concluir a operação.";
}

export default function ResourcePage<TItem extends { id: number }, TForm extends Record<string, unknown>>({
  title,
  subtitle,
  fields,
  initialForm,
  columns,
  list,
  create,
  update,
  remove,
  readOnly,
}: ResourcePageProps<TItem, TForm>) {
  const [items, setItems] = useState<TItem[]>([]);
  const [form, setForm] = useState<TForm>(initialForm);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  const canSave = useMemo(() => Boolean(create || update), [create, update]);

  async function load() {
    setLoading(true);
    setError("");
    try {
      setItems(await list());
    } catch (currentError) {
      setError(normalizeError(currentError));
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, []);

  function updateField(field: keyof TForm, value: string, type?: FieldConfig<TForm>["type"]) {
    setForm((current) => ({
      ...current,
      [field]: type === "number" ? Number(value) : value,
    }));
  }

  function edit(item: TItem) {
    const nextForm = { ...initialForm };
    fields.forEach((field) => {
      nextForm[field.name] = item[field.name as unknown as keyof TItem] as unknown as TForm[keyof TForm];
    });
    setForm(nextForm);
    setEditingId(item.id);
  }

  function resetForm() {
    setForm(initialForm);
    setEditingId(null);
  }

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    if (!canSave) return;

    setSaving(true);
    setError("");
    try {
      if (editingId && update) {
        await update(editingId, form);
      } else if (create) {
        await create(form);
      }
      resetForm();
      await load();
    } catch (currentError) {
      setError(normalizeError(currentError));
    } finally {
      setSaving(false);
    }
  }

  async function handleRemove(id: number) {
    if (!remove || !window.confirm("Confirmar exclusão?")) return;

    setError("");
    try {
      await remove(id);
      await load();
    } catch (currentError) {
      setError(normalizeError(currentError));
    }
  }

  return (
    <section className="page-stack">
      <header className="page-header">
        <div>
          <p className="eyebrow">Gestão</p>
          <h1>{title}</h1>
          <p>{subtitle}</p>
        </div>
        <button className="ghost-button" type="button" onClick={load}>
          Atualizar
        </button>
      </header>

      {error && <p className="alert">{error}</p>}

      {!readOnly && canSave && (
        <form className="panel form-grid" onSubmit={handleSubmit}>
          {fields.map((field) => (
            <label className={field.type === "textarea" ? "field wide" : "field"} key={String(field.name)}>
              <span>{field.label}</span>
              {field.type === "textarea" ? (
                <textarea
                  required={field.required}
                  value={String(form[field.name] ?? "")}
                  onChange={(event) => updateField(field.name, event.target.value, field.type)}
                />
              ) : field.type === "select" ? (
                <select
                  required={field.required}
                  value={String(form[field.name] ?? "")}
                  onChange={(event) => updateField(field.name, event.target.value, field.type)}
                >
                  <option value="">Selecione</option>
                  {field.options?.map((option) => (
                    <option key={option} value={option}>
                      {option}
                    </option>
                  ))}
                </select>
              ) : (
                <input
                  min={field.min}
                  required={field.required}
                  type={field.type ?? "text"}
                  value={String(form[field.name] ?? "")}
                  onChange={(event) => updateField(field.name, event.target.value, field.type)}
                />
              )}
            </label>
          ))}

          <div className="form-actions wide">
            <button className="primary-button" disabled={saving} type="submit">
              {editingId ? "Salvar alterações" : "Criar"}
            </button>
            {editingId && (
              <button className="ghost-button" type="button" onClick={resetForm}>
                Cancelar
              </button>
            )}
          </div>
        </form>
      )}

      <div className="panel table-wrap">
        {loading ? (
          <p>Carregando...</p>
        ) : (
          <table>
            <thead>
              <tr>
                {columns.map((column) => (
                  <th key={String(column.key)}>{column.label}</th>
                ))}
                {(update || remove) && <th>Ações</th>}
              </tr>
            </thead>
            <tbody>
              {items.map((item) => (
                <tr key={item.id}>
                  {columns.map((column) => (
                    <td key={String(column.key)}>{column.render ? column.render(item) : String(item[column.key] ?? "")}</td>
                  ))}
                  {(update || remove) && (
                    <td className="row-actions">
                      {update && (
                        <button className="ghost-button compact" type="button" onClick={() => edit(item)}>
                          Editar
                        </button>
                      )}
                      {remove && (
                        <button className="danger-button compact" type="button" onClick={() => handleRemove(item.id)}>
                          Excluir
                        </button>
                      )}
                    </td>
                  )}
                </tr>
              ))}
              {!items.length && (
                <tr>
                  <td colSpan={columns.length + 1}>Nenhum registro encontrado.</td>
                </tr>
              )}
            </tbody>
          </table>
        )}
      </div>
    </section>
  );
}
