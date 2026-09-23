"use client";

import { Alert, Button, Chip, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { CrudActions } from "@/components/tables/CrudActions";
import { useTenant } from "@/features/auth/TenantProvider";
import { listProcedureCategories, setProcedureCategoryActive } from "./procedureCategoriesApi";

export function ProcedureCategoriesList() {
  const { activeTenantId } = useTenant();
  const router = useRouter();
  const page = Math.max(1, Number(useSearchParams().get("page") ?? "1") || 1);
  const queryClient = useQueryClient();
  const query = useQuery({ queryKey: ["tenant", activeTenantId, "procedure-categories", "list", page], queryFn: ({ signal }) => listProcedureCategories(page, 20, undefined, signal), enabled: Boolean(activeTenantId) });
  const status = useMutation({ mutationFn: ({ id, active }: { id: string; active: boolean }) => setProcedureCategoryActive(id, active), onSuccess: () => queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "procedure-categories"] }) });
  const actionsCell = { minWidth: 190, whiteSpace: "nowrap", textAlign: "right" } as const;

  return <Stack spacing={3}>
    <Stack direction={{ xs: "column", sm: "row" }} spacing={2} sx={{ justifyContent: "space-between", alignItems: { sm: "center" } }}><Typography component="h1" variant="h4">Categorias</Typography><Button component={Link} href="/app/categorias/novo" variant="contained">Nova categoria</Button></Stack>
    {query.isError ? <Alert severity="error">Não foi possível carregar categorias.</Alert> : null}
    <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}><Table sx={{ width: "100%", minWidth: 760 }}><TableHead><TableRow><TableCell>Nome</TableCell><TableCell>Descrição</TableCell><TableCell>Status</TableCell><TableCell align="right" sx={actionsCell}>Ações</TableCell></TableRow></TableHead><TableBody>{query.data?.items.map(item => <TableRow key={item.id} hover onDoubleClick={() => router.push(`/app/categorias/${item.id}`)} sx={{ cursor: "pointer" }}><TableCell><Typography sx={{ fontWeight: 600 }}>{item.name}</Typography></TableCell><TableCell>{item.description ?? "—"}</TableCell><TableCell><Chip size="small" color={item.active ? "success" : "default"} label={item.active ? "Ativa" : "Inativa"} /></TableCell><TableCell align="right" sx={actionsCell}><CrudActions><Button component={Link} href={`/app/categorias/${item.id}`}>Visualizar</Button><Button disabled={status.isPending} onClick={() => status.mutate({ id: item.id, active: !item.active })}>{item.active ? "Inativar" : "Ativar"}</Button></CrudActions></TableCell></TableRow>)}</TableBody></Table></Paper>
  </Stack>;
}
