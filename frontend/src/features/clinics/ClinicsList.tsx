"use client";

import { Alert, Button, Chip, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { CrudActions } from "@/components/tables/CrudActions";
import { useTenant } from "@/features/auth/TenantProvider";
import { listClinics, setClinicActive } from "./clinicsApi";

export function ClinicsList() {
  const { activeTenantId } = useTenant();
  const router = useRouter();
  const page = Math.max(1, Number(useSearchParams().get("page") ?? "1") || 1);
  const queryClient = useQueryClient();
  const query = useQuery({ queryKey: ["tenant", activeTenantId, "clinics", "list", page], queryFn: ({ signal }) => listClinics(page, 20, undefined, undefined, signal), enabled: Boolean(activeTenantId) });
  const status = useMutation({ mutationFn: ({ id, active }: { id: string; active: boolean }) => setClinicActive(id, active), onSuccess: () => queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "clinics"] }) });
  const actionsCell = { minWidth: 190, whiteSpace: "nowrap", textAlign: "right" } as const;

  return <Stack spacing={3}>
    <Stack direction={{ xs: "column", sm: "row" }} spacing={2} sx={{ justifyContent: "space-between", alignItems: { sm: "center" } }}><Typography component="h1" variant="h4">Clinicas</Typography><Button component={Link} href="/app/clinicas/novo" variant="contained">Nova clinica</Button></Stack>
    {query.isError ? <Alert severity="error">Nao foi possivel carregar clinicas.</Alert> : null}
    <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}><Table sx={{ width: "100%", minWidth: 760 }}><TableHead><TableRow><TableCell>Nome</TableCell><TableCell>Documento</TableCell><TableCell>Status</TableCell><TableCell align="right" sx={actionsCell}>Acoes</TableCell></TableRow></TableHead><TableBody>{query.data?.items.map(item => <TableRow key={item.id} hover onDoubleClick={() => router.push(`/app/clinicas/${item.id}`)} sx={{ cursor: "pointer" }}><TableCell><Typography sx={{ fontWeight: 600 }}>{item.name}</Typography>{item.legalName ? <Typography variant="body2" color="text.secondary">{item.legalName}</Typography> : null}</TableCell><TableCell>{item.document ?? "—"}</TableCell><TableCell><Chip size="small" color={item.active ? "success" : "default"} label={item.active ? "Ativa" : "Inativa"} /></TableCell><TableCell align="right" sx={actionsCell}><CrudActions><Button component={Link} href={`/app/clinicas/${item.id}`}>Visualizar</Button><Button disabled={status.isPending} onClick={() => status.mutate({ id: item.id, active: !item.active })}>{item.active ? "Inativar" : "Ativar"}</Button></CrudActions></TableCell></TableRow>)}</TableBody></Table></Paper>
  </Stack>;
}
