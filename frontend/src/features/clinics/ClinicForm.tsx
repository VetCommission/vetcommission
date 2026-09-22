"use client";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Alert, Button, Paper, Stack, TextField, Typography } from "@mui/material";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState, type MouseEvent } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { useTenant } from "@/features/auth/TenantProvider";
import { saveClinic, type Clinic } from "./clinicsApi";

const schema = z.object({
  name: z.string().trim().min(1, "Informe o nome.").max(160),
  legalName: z.string().max(200).optional(), document: z.string().max(30).optional(),
  email: z.string().email("Informe um e-mail valido.").optional().or(z.literal("")), phone: z.string().max(40).optional(),
});
type FormData = z.infer<typeof schema>;

export function ClinicForm({ clinic, initialMode = "view" }: { clinic?: Clinic; initialMode?: "create" | "view" | "edit" }) {
  const router = useRouter(); const queryClient = useQueryClient(); const { activeTenantId } = useTenant();
  const [mode, setMode] = useState(clinic ? initialMode : "create"); const [error, setError] = useState<string | null>(null);
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormData>({ resolver: zodResolver(schema), defaultValues: { name: clinic?.name ?? "", legalName: clinic?.legalName ?? "", document: clinic?.document ?? "", email: clinic?.email ?? "", phone: clinic?.phone ?? "" } });
  const mutation = useMutation({ mutationFn: (data: FormData) => saveClinic(data, clinic?.id), onSuccess: async () => { await queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "clinics"] }); router.replace("/app/clinicas"); }, onError: () => setError("Nao foi possivel salvar a clinica.") });
  const edit = (event: MouseEvent<HTMLButtonElement>) => { event.preventDefault(); event.stopPropagation(); setMode("edit"); };
  return (
    <Paper variant="outlined" sx={{ width: "100%", p: { xs: 3, sm: 4, md: 5 } }}>
      <Stack component="form" spacing={3} onSubmit={handleSubmit(data => { setError(null); mutation.mutate(data); })} noValidate>
        <Typography variant="h4">{mode === "create" ? "Nova clinica" : "Clinica"}</Typography>
        {error ? <Alert severity="error">{error}</Alert> : null}
        <Stack direction={{ xs: "column", md: "row" }} spacing={2}><TextField fullWidth disabled={mode === "view"} label="Nome" error={Boolean(errors.name)} helperText={errors.name?.message} {...register("name")} /><TextField fullWidth disabled={mode === "view"} label="Nome juridico" {...register("legalName")} /></Stack>
        <Stack direction={{ xs: "column", md: "row" }} spacing={2}><TextField fullWidth disabled={mode === "view"} label="Documento" {...register("document")} /><TextField fullWidth disabled={mode === "view"} label="E-mail" error={Boolean(errors.email)} helperText={errors.email?.message} {...register("email")} /><TextField fullWidth disabled={mode === "view"} label="Telefone" {...register("phone")} /></Stack>
        <Stack direction="row" spacing={2} sx={{ justifyContent: "flex-end", pt: 2, borderTop: "1px solid", borderColor: "divider" }}><Button component={Link} href="/app/clinicas" type="button">Voltar</Button>{mode === "view" ? <Button type="button" variant="contained" onClick={edit}>Editar</Button> : <Button type="submit" variant="contained" disabled={isSubmitting}>Salvar</Button>}</Stack>
      </Stack>
    </Paper>
  );
}
