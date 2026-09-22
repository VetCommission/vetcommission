"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Alert, Button, Paper, Stack, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { saveProfessionalRole } from "@/features/professional-roles/professionalRolesApi";
import { saveProfessionalSpecialty } from "@/features/professional-specialties/professionalSpecialtiesApi";
import { getApiErrorMessage, getApiFieldErrors } from "@/lib/api/apiError";

const schema = z.object({ name: z.string().trim().min(1, "Informe o nome.").max(120) });
type FormData = z.infer<typeof schema>;

export function MasterDataForm({ kind, id, initialName = "", initialMode = "create" }: { kind: "role" | "specialty"; id?: string; initialName?: string; initialMode?: "create" | "view" | "edit" }) {
  const router = useRouter();
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [mode, setMode] = useState(initialMode);
  const {
    register,
    handleSubmit,
    reset,
    setError,
    setFocus,
    formState: { errors, isSubmitting },
  } = useForm<FormData>({ resolver: zodResolver(schema), defaultValues: { name: initialName } });

  const submit = async (data: FormData) => {
    setSubmitError(null);
    try {
      await (kind === "role"
        ? saveProfessionalRole(data.name, id)
        : saveProfessionalSpecialty(data.name, id));
      reset({ name: "" });
      router.replace(kind === "role" ? "/app/funcoes-cargos" : "/app/especialidades");
    } catch (error) {
      getApiFieldErrors(error).forEach(({ field, message }) => {
        if (field === "name") {
          setError("name", { type: "server", message });
          setFocus("name");
        }
      });
      setSubmitError(getApiErrorMessage(error, "Não foi possível salvar o cadastro."));
    }
  };

  return (
    <Paper variant="outlined" sx={{ width: "100%", p: 4 }}>
      <Stack component="form" spacing={2} onSubmit={handleSubmit(submit)} noValidate>
        <Typography component="h1" variant="h4">
          {kind === "role" ? "Nova função ou cargo" : "Nova especialidade"}
        </Typography>
        {submitError ? <Alert severity="error">{submitError}</Alert> : null}
        <TextField disabled={mode === "view"}
          label="Nome"
          error={Boolean(errors.name)}
          helperText={errors.name?.message}
          {...register("name")}
        />
        <Stack direction="row" spacing={2} sx={{ justifyContent: "flex-end" }}>
          <Button type="button" onClick={() => router.back()}>
            Voltar
          </Button>
          {mode === "view" ? <Button type="button" variant="contained" onClick={() => setMode("edit")}>Editar</Button> : <Button disabled={isSubmitting} type="submit" variant="contained">
            {isSubmitting ? "Salvando…" : "Salvar"}
          </Button>}
        </Stack>
      </Stack>
    </Paper>
  );
}
