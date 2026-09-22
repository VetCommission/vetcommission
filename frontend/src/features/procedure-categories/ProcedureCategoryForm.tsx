"use client";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Alert, TextField } from "@mui/material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { useTenant } from "@/features/auth/TenantProvider";
import { saveProcedureCategory, type ProcedureCategory } from "./procedureCategoriesApi";
import { CrudFormShell } from "@/components/forms/CrudFormShell";

const schema = z.object({
  name: z.string().trim().min(1, "Informe o nome.").max(160),
  description: z.string().max(500).optional(),
});
type FormData = z.infer<typeof schema>;
type Mode = "create" | "view" | "edit";

export function ProcedureCategoryForm({ category, initialMode = "view" }: { category?: ProcedureCategory; initialMode?: Mode }) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { activeTenantId } = useTenant();
  const [mode, setMode] = useState<Mode>(category ? initialMode : "create");
  const [error, setError] = useState<string | null>(null);
  const form = useForm<FormData>({ resolver: zodResolver(schema), defaultValues: { name: category?.name ?? "", description: category?.description ?? "" } });
  const mutation = useMutation({ mutationFn: (value: FormData) => saveProcedureCategory(value, category?.id), onSuccess: async () => { await queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "procedure-categories"] }); router.replace("/app/categorias"); }, onError: () => setError("Nao foi possivel salvar a categoria.") });
  return <CrudFormShell title={mode === "create" ? "Nova categoria" : "Categoria"} mode={mode} listHref="/app/categorias" isSubmitting={form.formState.isSubmitting || mutation.isPending} onEdit={() => setMode("edit")} onSubmit={form.handleSubmit(value => mutation.mutate(value))}>{error ? <Alert severity="error">{error}</Alert> : null}<TextField fullWidth disabled={mode === "view"} label="Nome" error={Boolean(form.formState.errors.name)} helperText={form.formState.errors.name?.message} {...form.register("name")} /><TextField fullWidth multiline minRows={3} disabled={mode === "view"} label="Descricao" {...form.register("description")} /></CrudFormShell>;
}
