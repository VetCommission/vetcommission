"use client";

import { Alert, Button, Chip, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { CrudActions } from "@/components/tables/CrudActions";
import { useTenant } from "@/features/auth/TenantProvider";
import { listProcedures, setProcedureActive } from "./proceduresApi";

export function ProceduresList() {
  const { activeTenantId } = useTenant();
  const router = useRouter();
  const queryClient = useQueryClient();
  const query = useQuery({ queryKey: ["tenant", activeTenantId, "procedures", "list"], queryFn: ({ signal }) => listProcedures(1, 100, undefined, undefined, signal), enabled: Boolean(activeTenantId) });
  const status = useMutation({ mutationFn: ({ id, active }: { id: string; active: boolean }) => setProcedureActive(id, active), onSuccess: () => queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "procedures"] }) });
  const actionsCell = { minWidth: 190, whiteSpace: "nowrap", textAlign: "right" } as const;

  return <Stack spacing={3}>
    <Stack direction={{ xs: "column", sm: "row" }} spacing={2} sx={{ justifyContent: "space-between", alignItems: { sm: "center" } }}><Typography component="h1" variant="h4">Procedimentos</Typography><Button component={Link} href="/app/procedimentos/novo" variant="contained">Novo procedimento</Button></Stack>
    {query.isError ? <Alert severity="error">Não foi possível carregar procedimentos.</Alert> : null}
    <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}><Table sx={{ width: "100%", minWidth: 760 }}><TableHead><TableRow><TableCell>Nome</TableCell><TableCell>Código</TableCell><TableCell>Valor</TableCell><TableCell>Status</TableCell><TableCell align="right" sx={actionsCell}>Ações</TableCell></TableRow></TableHead><TableBody>{query.data?.items.map(item => <TableRow key={item.id} hover onDoubleClick={() => router.push(`/app/procedimentos/${item.id}`)} sx={{ cursor: "pointer" }}><TableCell><Typography sx={{ fontWeight: 600 }}>{item.name}</Typography></TableCell><TableCell>{item.code ?? "—"}</TableCell><TableCell>R$ {item.defaultValue.toFixed(2)}</TableCell><TableCell><Chip size="small" color={item.active ? "success" : "default"} label={item.active ? "Ativo" : "Inativo"} /></TableCell><TableCell align="right" sx={actionsCell}><CrudActions><Button component={Link} href={`/app/procedimentos/${item.id}`}>Visualizar</Button><Button disabled={status.isPending} onClick={() => status.mutate({ id: item.id, active: !item.active })}>{item.active ? "Inativar" : "Ativar"}</Button></CrudActions></TableCell></TableRow>)}</TableBody></Table></Paper>
  </Stack>;
}
