"use client";

import { Alert, Box, MenuItem, TextField } from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useForm, useWatch } from "react-hook-form";
import { useTenant } from "@/features/auth/TenantProvider";
import { listProcedures } from "@/features/procedures/proceduresApi";
import { CrudFormShell, type CrudFormMode } from "@/components/forms/CrudFormShell";
import { getApiErrorMessage } from "@/lib/api/apiError";
import { listRules, saveRule, type Rule } from "./commissionApi";

type Data = { procedureId: string; ruleType: "percentage" | "fixed"; percentage?: number; fixedValue?: number };

export function CommissionRuleForm({ competencyId, initialRule, locked = false }: { competencyId: string; initialRule?: Rule; locked?: boolean }) {
  const router = useRouter();
  const { activeTenantId } = useTenant();
  const queryClient = useQueryClient();
  const [mode, setMode] = useState<CrudFormMode>(initialRule ? "view" : "create");
  const [error, setError] = useState<string | null>(null);
  const form = useForm<Data>({
    defaultValues: {
      procedureId: initialRule?.procedureId ?? "",
      ruleType: initialRule?.ruleType === "fixed" ? "fixed" : "percentage",
      percentage: initialRule?.percentage ?? undefined,
      fixedValue: initialRule?.fixedValue ?? undefined,
    },
  });
  const type = useWatch({ control: form.control, name: "ruleType" });
  const procedures = useQuery({
    queryKey: ["tenant", activeTenantId, "procedures", "commission-rules"],
    queryFn: ({ signal }) => listProcedures(1, 100, undefined, undefined, signal),
    enabled: Boolean(activeTenantId),
  });
  const mutation = useMutation({
    mutationFn: (value: Data) => saveRule(competencyId, {
      procedureId: value.procedureId,
      ruleType: value.ruleType,
      percentage: value.ruleType === "percentage" ? value.percentage ?? null : null,
      fixedValue: value.ruleType === "fixed" ? value.fixedValue ?? null : null,
    }, initialRule?.id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "competency", competencyId, "rules"] });
      router.push(`/app/competencias/${competencyId}`);
    },
    onError: (cause) => setError(getApiErrorMessage(cause, "Nao foi possivel salvar a regra.")),
  });
  const readOnly = mode === "view" || locked;
  const submit = (value: Data) => {
    setError(null);
    const amount = value.ruleType === "percentage" ? value.percentage : value.fixedValue;
    if (!value.procedureId) {
      setError("Selecione um procedimento.");
      return;
    }
    if (amount === undefined || Number.isNaN(amount) || amount <= 0 || (value.ruleType === "percentage" && amount > 100)) {
      setError(value.ruleType === "percentage" ? "Informe um percentual entre 0 e 100." : "Informe um valor fixo maior que zero.");
      return;
    }
    mutation.mutate(value);
  };

  return <CrudFormShell title={initialRule ? "Regra de comissao" : "Nova regra de comissao"} mode={mode} listHref={`/app/competencias/${competencyId}`} isSubmitting={mutation.isPending} canEdit={!locked} onEdit={() => setMode("edit")} onSubmit={form.handleSubmit(submit)}>
    {locked ? <Alert severity="info">A competencia esta fechada. Esta regra nao pode ser alterada.</Alert> : null}
    {error ? <Alert severity="error">{error}</Alert> : null}
    <Box sx={{ display: "grid", gridTemplateColumns: { xs: "1fr", md: "minmax(0, 2fr) minmax(0, 1fr) minmax(0, 1fr)" }, gap: 2 }}>
      <TextField select fullWidth defaultValue={initialRule?.procedureId ?? ""} disabled={readOnly || procedures.isLoading} label="Procedimento" {...form.register("procedureId")}>
        {procedures.data?.items.map(item => <MenuItem key={item.id} value={item.id}>{item.name}</MenuItem>) ?? <MenuItem disabled value="">Nenhum procedimento disponivel</MenuItem>}
      </TextField>
      <TextField select fullWidth disabled={readOnly} label="Tipo" defaultValue={initialRule?.ruleType === "fixed" ? "fixed" : "percentage"} {...form.register("ruleType")}>
        <MenuItem value="percentage">Percentual</MenuItem><MenuItem value="fixed">Valor fixo</MenuItem>
      </TextField>
      {type === "percentage" ? <TextField fullWidth disabled={readOnly} type="number" label="Percentual" slotProps={{ htmlInput: { min: 0, max: 100, step: .01 } }} {...form.register("percentage", { valueAsNumber: true })} /> : <TextField fullWidth disabled={readOnly} type="number" label="Valor fixo" slotProps={{ htmlInput: { min: 0, step: .01 } }} {...form.register("fixedValue", { valueAsNumber: true })} />}
    </Box>
  </CrudFormShell>;
}

export function CommissionRuleEditPage({ competencyId, ruleId, locked }: { competencyId: string; ruleId?: string; locked: boolean }) {
  const { activeTenantId } = useTenant();
  const rules = useQuery({ queryKey: ["tenant", activeTenantId, "competency", competencyId, "rules"], queryFn: ({ signal }) => listRules(competencyId, signal), enabled: Boolean(activeTenantId && competencyId) });
  if (!ruleId) return <CommissionRuleForm competencyId={competencyId} locked={locked} />;
  const rule = rules.data?.find(item => item.id === ruleId);
  if (rules.isLoading) return null;
  return rule ? <CommissionRuleForm competencyId={competencyId} initialRule={rule} locked={locked} /> : <Alert severity="error">Regra nao encontrada.</Alert>;
}
