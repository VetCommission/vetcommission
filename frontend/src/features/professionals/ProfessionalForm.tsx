"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Alert, Button, MenuItem, Paper, Select, Stack, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { ApiError } from "@/lib/api/apiError";
import type { Professional } from "@/types/api";
import { createProfessional, listProfessionalRoles, listProfessionalSpecialties, updateProfessional, type ProfessionalInput } from "./professionalsApi";
import { professionalQueryKeys } from "./professionalQueryKeys";
import { useTenant } from "@/features/auth/TenantProvider";

const schema = z.object({
  name: z.string().trim().min(1, "Informe o nome.").max(160),
  email: z.string().trim().email("Informe um e-mail válido.").optional().or(z.literal("")),
  phone: z.string().trim().max(40).optional(),
  role: z.string().trim().min(1, "Selecione a função ou cargo.").max(120),
  professionalRegistration: z.string().trim().max(80).optional(),
  specialty: z.string().trim().max(120).optional(),
});

type FormData = z.infer<typeof schema>;
type FormMode = "create" | "view" | "edit";

export function ProfessionalForm({ professional, initialMode = "view" }: { professional?: Professional; initialMode?: FormMode }) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { activeTenantId } = useTenant();
  const [currentProfessional] = useState(professional);
  const [mode, setMode] = useState<FormMode>(professional ? initialMode : "create");
  const isReadOnly = mode === "view";
  const rolesQuery = useQuery({ queryKey: ["tenant", activeTenantId, "professional-roles", "list"], queryFn: ({ signal }) => listProfessionalRoles(signal), enabled: Boolean(activeTenantId) });
  const specialtiesQuery = useQuery({ queryKey: ["tenant", activeTenantId, "professional-specialties", "list"], queryFn: ({ signal }) => listProfessionalSpecialties(signal), enabled: Boolean(activeTenantId) });
  const mutation = useMutation({
    mutationFn: (input: ProfessionalInput) => currentProfessional ? updateProfessional(currentProfessional.id, input) : createProfessional(input),
    onSuccess: async () => { if (activeTenantId) await queryClient.invalidateQueries({ queryKey: professionalQueryKeys.all(activeTenantId) }); router.replace("/app/profissionais"); },
  });
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { name: currentProfessional?.name ?? "", email: currentProfessional?.email ?? "", phone: currentProfessional?.phone ?? "", role: currentProfessional?.role ?? "", professionalRegistration: currentProfessional?.professionalRegistration ?? "", specialty: currentProfessional?.specialty ?? "" },
  });

  return (
    <Paper variant="outlined" sx={{ p: { xs: 3, md: 4 } }}>
      <Stack component="form" spacing={2} onSubmit={handleSubmit((data) => mutation.mutate(data))}>
        <Typography component="h1" variant="h4">{mode === "create" ? "Novo profissional" : "Profissional"}</Typography>
        {mutation.isError ? <Alert severity="error">{mutation.error instanceof ApiError ? mutation.error.message : "Não foi possível salvar."}</Alert> : null}
        {rolesQuery.isError ? <Alert severity="error">Não foi possível carregar funções e cargos.</Alert> : null}
        {specialtiesQuery.isError ? <Alert severity="error">Não foi possível carregar especialidades.</Alert> : null}
        <TextField disabled={isReadOnly} label="Nome" error={Boolean(errors.name)} helperText={errors.name?.message} {...register("name")} />
        <TextField disabled={isReadOnly} label="E-mail" error={Boolean(errors.email)} helperText={errors.email?.message} {...register("email")} />
        <TextField disabled={isReadOnly} label="Telefone" {...register("phone")} />
        <Select disabled={isReadOnly || rolesQuery.isLoading} displayEmpty error={Boolean(errors.role)} defaultValue={currentProfessional?.role ?? ""} {...register("role")}>
          <MenuItem value="" disabled>Selecione a função ou cargo</MenuItem>
          {rolesQuery.data?.map((item) => <MenuItem key={item.id} value={item.name}>{item.name}</MenuItem>)}
        </Select>
        <TextField disabled={isReadOnly} label="Registro profissional" {...register("professionalRegistration")} />
        <Select disabled={isReadOnly || specialtiesQuery.isLoading} displayEmpty defaultValue={currentProfessional?.specialty ?? ""} {...register("specialty")}>
          <MenuItem value="">Selecione a especialidade</MenuItem>
          {specialtiesQuery.data?.map((item) => <MenuItem key={item.id} value={item.name}>{item.name}</MenuItem>)}
        </Select>
        <Stack direction="row" spacing={2} sx={{ justifyContent: "flex-end" }}>
          <Button type="button" onClick={() => router.push("/app/profissionais")}>Voltar</Button>
          {isReadOnly ? <Button type="button" onClick={(event) => { event.preventDefault(); setMode("edit"); }} variant="contained">Editar</Button> : <Button disabled={mutation.isPending} type="submit" variant="contained">{mode === "create" ? "Salvar cadastro" : "Salvar"}</Button>}
        </Stack>
      </Stack>
    </Paper>
  );
}
