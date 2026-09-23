"use client";

import { Alert, Box, TextField } from "@mui/material";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { useTenant } from "@/features/auth/TenantProvider";
import { CrudFormShell } from "@/components/forms/CrudFormShell";
import { saveCompetency } from "./commissionApi";

type Data = { year: number; month: number };

export function CompetencyForm() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { activeTenantId } = useTenant();
  const form = useForm<Data>({ defaultValues: { year: new Date().getFullYear(), month: new Date().getMonth() + 1 } });
  const mutation = useMutation({
    mutationFn: (value: Data) => saveCompetency(value),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "competencies"] });
      router.replace("/app/competencias");
    },
  });

  return <CrudFormShell title="Nova competencia" mode="create" listHref="/app/competencias" isSubmitting={mutation.isPending} onEdit={() => undefined} onSubmit={form.handleSubmit(value => mutation.mutate({ year: Number(value.year), month: Number(value.month) }))}>
    {mutation.isError ? <Alert severity="error">Nao foi possivel salvar a competencia.</Alert> : null}
    <Box sx={{ display: "grid", gridTemplateColumns: { xs: "1fr", sm: "repeat(2, minmax(0, 1fr))" }, gap: 2 }}>
      <TextField fullWidth type="number" label="Ano" {...form.register("year", { valueAsNumber: true })} />
      <TextField fullWidth type="number" label="Mes" slotProps={{ htmlInput: { min: 1, max: 12 } }} {...form.register("month", { valueAsNumber: true })} />
    </Box>
  </CrudFormShell>;
}
