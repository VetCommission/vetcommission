"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Alert, Button, Paper, Stack, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { saveProfessionalRole } from "@/features/professional-roles/professionalRolesApi";
import { saveProfessionalSpecialty } from "@/features/professional-specialties/professionalSpecialtiesApi";
import { ApiError } from "@/lib/api/apiError";

const schema = z.object({ name: z.string().trim().min(1, "Informe o nome.").max(120) });
type FormData = z.infer<typeof schema>;

export function MasterDataForm({ kind }: { kind: "role" | "specialty" }) {
  const router = useRouter();
  const [submitError, setSubmitError] = useState<string | null>(null);
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormData>({ resolver: zodResolver(schema), defaultValues: { name: "" } });
  const submit = async (data: FormData) => {
    setSubmitError(null);
    try {
      await (kind === "role"
        ? saveProfessionalRole(data.name)
        : saveProfessionalSpecialty(data.name));
      router.replace(kind === "role" ? "/app/funcoes-cargos" : "/app/especialidades");
    } catch (error) {
      setSubmitError(
        error instanceof ApiError ? error.message : "Não foi possível salvar o cadastro.",
      );
    }
  };

  return (
    <Paper variant="outlined" sx={{ p: 4 }}>
      <Stack component="form" spacing={2} onSubmit={handleSubmit(submit)} noValidate>
        <Typography component="h1" variant="h4">
          {kind === "role" ? "Nova função ou cargo" : "Nova especialidade"}
        </Typography>
        {submitError ? (
          <Alert severity="error" role="alert">
            {submitError}
          </Alert>
        ) : null}
        <TextField
          label="Nome"
          error={Boolean(errors.name)}
          helperText={errors.name?.message}
          aria-describedby={errors.name ? "master-data-name-error" : undefined}
          {...register("name")}
        />
        <Stack direction="row" spacing={2} sx={{ justifyContent: "flex-end" }}>
          <Button type="button" onClick={() => router.back()}>
            Voltar
          </Button>
          <Button disabled={isSubmitting} type="submit" variant="contained">
            {isSubmitting ? "Salvando…" : "Salvar"}
          </Button>
        </Stack>
      </Stack>
    </Paper>
  );
}
