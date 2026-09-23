"use client";

import { Alert, Box, Button, Chip, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { useTenant } from "@/features/auth/TenantProvider";
import { listProcedures } from "@/features/procedures/proceduresApi";
import { listRules } from "./commissionApi";
import { CrudActions } from "@/components/tables/CrudActions";

export function CommissionRulesList({ competencyId, locked }: { competencyId: string; locked: boolean }) {
  const { activeTenantId } = useTenant();
  const rules = useQuery({ queryKey: ["tenant", activeTenantId, "competency", competencyId, "rules"], queryFn: ({ signal }) => listRules(competencyId, signal), enabled: Boolean(activeTenantId && competencyId) });
  const procedures = useQuery({ queryKey: ["tenant", activeTenantId, "procedures", "commission-rules"], queryFn: ({ signal }) => listProcedures(1, 100, undefined, undefined, signal), enabled: Boolean(activeTenantId) });
  const names = new Map((procedures.data?.items ?? []).map(item => [item.id, item.name]));

  return <Stack spacing={2}>
    <Stack direction={{ xs: "column", sm: "row" }} spacing={2} sx={{ justifyContent: "space-between", alignItems: { sm: "center" } }}>
      <BoxTitle locked={locked} />
      {!locked ? <Button component={Link} href={`/app/competencias/${competencyId}/regras/novo`} variant="contained">Nova regra</Button> : null}
    </Stack>
    {rules.isError ? <Alert severity="error">Nao foi possivel carregar as regras.</Alert> : null}
    <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}><Table sx={{ width: "100%", minWidth: 760 }}>
      <TableHead><TableRow><TableCell>Procedimento</TableCell><TableCell>Tipo</TableCell><TableCell>Valor</TableCell><TableCell>Status</TableCell><TableCell align="right">Acoes</TableCell></TableRow></TableHead>
      <TableBody>{rules.data?.map(rule => <TableRow key={rule.id} hover>
        <TableCell><Typography sx={{ fontWeight: 600 }}>{names.get(rule.procedureId) ?? rule.procedureId}</Typography></TableCell>
        <TableCell>{rule.ruleType === "percentage" ? "Percentual" : "Valor fixo"}</TableCell>
        <TableCell>{rule.ruleType === "percentage" ? `${rule.percentage ?? 0}%` : `R$ ${(rule.fixedValue ?? 0).toFixed(2)}`}</TableCell>
        <TableCell><Chip size="small" color={rule.active ? "success" : "default"} label={rule.active ? "Ativa" : "Inativa"} /></TableCell>
        <TableCell align="right"><CrudActions><Button component={Link} href={`/app/competencias/${competencyId}/regras/novo?ruleId=${rule.id}`}>Visualizar</Button></CrudActions></TableCell>
      </TableRow>)}</TableBody>
    </Table></Paper>
  </Stack>;
}

function BoxTitle({ locked }: { locked: boolean }) {
  return <Box><Typography component="h1" variant="h4">Regras de comissao</Typography><Typography color="text.secondary">{locked ? "Competencia fechada; regras somente para consulta." : "Configure a regra geral por procedimento."}</Typography></Box>;
}
