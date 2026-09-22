"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Alert,
  Button,
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { useTenant } from "@/features/auth/TenantProvider";
import { listProfessionalRoles } from "@/features/professional-roles/professionalRolesApi";
import { listProfessionalSpecialties } from "@/features/professional-specialties/professionalSpecialtiesApi";
import { getApiErrorMessage, getApiFieldErrors } from "@/lib/api/apiError";
import { professionalQueryKeys } from "./professionalQueryKeys";
import type { Professional, ProfessionalInput } from "./professionalTypes";
import { createProfessional, updateProfessional } from "./professionalsApi";

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

export function ProfessionalForm({
  professional,
  initialMode = "view",
}: {
  professional?: Professional;
  initialMode?: FormMode;
}) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { activeTenantId } = useTenant();
  const [mode, setMode] = useState<FormMode>(professional ? initialMode : "create");
  const [submitError, setSubmitError] = useState<string | null>(null);
  const isReadOnly = mode === "view";
  const rolesQuery = useQuery({
    queryKey: ["tenant", activeTenantId, "professional-roles", "list", 1],
    queryFn: ({ signal }) => listProfessionalRoles(1, 100, signal),
    enabled: Boolean(activeTenantId),
  });
  const specialtiesQuery = useQuery({
    queryKey: ["tenant", activeTenantId, "professional-specialties", "list", 1],
    queryFn: ({ signal }) => listProfessionalSpecialties(1, 100, signal),
    enabled: Boolean(activeTenantId),
  });
  const {
    register,
    handleSubmit,
    reset,
    setError,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: professional?.name ?? "",
      email: professional?.email ?? "",
      phone: professional?.phone ?? "",
      role: professional?.role ?? "",
      professionalRegistration: professional?.professionalRegistration ?? "",
      specialty: professional?.specialty ?? "",
    },
  });
  const mutation = useMutation({
    mutationFn: (input: ProfessionalInput) =>
      professional ? updateProfessional(professional.id, input) : createProfessional(input),
    onSuccess: async (savedProfessional) => {
      reset({
        name: savedProfessional.name,
        email: savedProfessional.email ?? "",
        phone: savedProfessional.phone ?? "",
        role: savedProfessional.role,
        professionalRegistration: savedProfessional.professionalRegistration ?? "",
        specialty: savedProfessional.specialty ?? "",
      });
      if (activeTenantId) {
        await queryClient.invalidateQueries({
          queryKey: professionalQueryKeys.all(activeTenantId),
        });
      }
      router.replace("/app/profissionais");
    },
    onError: (error) => {
      getApiFieldErrors(error).forEach(({ field, message }) => {
        if (
          ["name", "email", "phone", "role", "professionalRegistration", "specialty"].includes(
            field,
          )
        ) {
          setError(field as keyof FormData, { type: "server", message });
        }
      });
      setSubmitError(getApiErrorMessage(error, "Não foi possível salvar o profissional."));
    },
  });

  return (
    <Paper variant="outlined" sx={{ p: { xs: 3, md: 4 } }}>
      <Stack
        component="form"
        spacing={2}
        onSubmit={handleSubmit((data) => {
          setSubmitError(null);
          mutation.mutate(data);
        })}
        noValidate
      >
        <Typography component="h1" variant="h4">
          {mode === "create" ? "Novo profissional" : "Profissional"}
        </Typography>
        {submitError ? <Alert severity="error">{submitError}</Alert> : null}
        {rolesQuery.isError ? (
          <Alert severity="error">Não foi possível carregar funções e cargos.</Alert>
        ) : null}
        {specialtiesQuery.isError ? (
          <Alert severity="error">Não foi possível carregar especialidades.</Alert>
        ) : null}
        <TextField
          disabled={isReadOnly}
          label="Nome"
          error={Boolean(errors.name)}
          helperText={errors.name?.message}
          {...register("name")}
        />
        <TextField
          disabled={isReadOnly}
          label="E-mail"
          error={Boolean(errors.email)}
          helperText={errors.email?.message}
          {...register("email")}
        />
        <TextField
          disabled={isReadOnly}
          label="Telefone"
          error={Boolean(errors.phone)}
          helperText={errors.phone?.message}
          {...register("phone")}
        />
        <FormControl disabled={isReadOnly || rolesQuery.isLoading} error={Boolean(errors.role)}>
          <InputLabel id="professional-role-label">Função ou cargo</InputLabel>
          <Select
            labelId="professional-role-label"
            label="Função ou cargo"
            defaultValue={professional?.role ?? ""}
            {...register("role")}
          >
            <MenuItem value="" disabled>
              Selecione a função ou cargo
            </MenuItem>
            {rolesQuery.data?.items.map((item) => (
              <MenuItem key={item.id} value={item.name}>
                {item.name}
              </MenuItem>
            ))}
          </Select>
          <FormHelperText>{errors.role?.message}</FormHelperText>
        </FormControl>
        <TextField
          disabled={isReadOnly}
          label="Registro profissional"
          error={Boolean(errors.professionalRegistration)}
          helperText={errors.professionalRegistration?.message}
          {...register("professionalRegistration")}
        />
        <FormControl
          disabled={isReadOnly || specialtiesQuery.isLoading}
          error={Boolean(errors.specialty)}
        >
          <InputLabel id="professional-specialty-label">Especialidade</InputLabel>
          <Select
            labelId="professional-specialty-label"
            label="Especialidade"
            defaultValue={professional?.specialty ?? ""}
            {...register("specialty")}
          >
            <MenuItem value="">Selecione a especialidade</MenuItem>
            {specialtiesQuery.data?.items.map((item) => (
              <MenuItem key={item.id} value={item.name}>
                {item.name}
              </MenuItem>
            ))}
          </Select>
          <FormHelperText>{errors.specialty?.message}</FormHelperText>
        </FormControl>
        <Stack direction="row" spacing={2} sx={{ justifyContent: "flex-end" }}>
          <Button component={Link} href="/app/profissionais" type="button">
            Voltar
          </Button>
          {isReadOnly ? (
            <Button type="button" onClick={() => setMode("edit")} variant="contained">
              Editar
            </Button>
          ) : (
            <Button disabled={mutation.isPending} type="submit" variant="contained">
              {mutation.isPending ? "Salvando…" : mode === "create" ? "Salvar cadastro" : "Salvar"}
            </Button>
          )}
        </Stack>
      </Stack>
    </Paper>
  );
}
