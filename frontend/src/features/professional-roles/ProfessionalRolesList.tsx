"use client";

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Alert,
  Button,
  Chip,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { useRouter } from "next/navigation";
import { listProfessionalRoles, setProfessionalRoleActive } from "./professionalRolesApi";
import { useTenant } from "@/features/auth/TenantProvider";
import { CrudActions } from "@/components/tables/CrudActions";

export function ProfessionalRolesList() {
  const { activeTenantId } = useTenant();
  const router = useRouter();
  const queryClient = useQueryClient();
  const statusMutation = useMutation({ mutationFn: ({ id, active }: { id: string; active: boolean }) => setProfessionalRoleActive(id, active), onSuccess: () => queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "professional-roles"] }) });
  const page = Math.max(1, Number(useSearchParams().get("page") ?? "1") || 1);
  const query = useQuery({
    queryKey: ["tenant", activeTenantId, "professional-roles", "list", page],
    queryFn: ({ signal }) => listProfessionalRoles(page, 20, signal),
    enabled: Boolean(activeTenantId),
  });
  return (
    <Stack spacing={3}>
      <Stack direction="row" sx={{ justifyContent: "space-between", alignItems: "center" }}>
        <Typography component="h1" variant="h4">
          Funções e cargos
        </Typography>
        <Button component={Link} href="/app/funcoes-cargos/novo" variant="contained">
          Novo cadastro
        </Button>
      </Stack>
      {query.isError ? (
        <Alert severity="error">Não foi possível carregar funções e cargos.</Alert>
      ) : null}
      <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}>
        <Table aria-label="Lista de funções e cargos" sx={{ width: "100%", minWidth: 760 }}>
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="right">Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {query.data?.items.map((item) => (
              <TableRow key={item.id} hover onDoubleClick={() => router.push(`/app/funcoes-cargos/${item.id}`)} sx={{ cursor: "pointer" }}>
                <TableCell>{item.name}</TableCell>
                <TableCell><Chip size="small" color={item.active ? "success" : "default"} label={item.active ? "Ativo" : "Inativo"} /></TableCell>
                <TableCell align="right"><CrudActions><Button component={Link} href={`/app/funcoes-cargos/${item.id}`}>Visualizar</Button><Button disabled={statusMutation.isPending} onClick={() => statusMutation.mutate({ id: item.id, active: !item.active })}>{item.active ? "Inativar" : "Ativar"}</Button></CrudActions></TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
      {query.data ? (
        <Stack direction="row" sx={{ justifyContent: "flex-end" }}>
          <Button
            component={Link}
            disabled={page <= 1}
            href={`/app/funcoes-cargos?page=${page - 1}`}
          >
            Anterior
          </Button>
          <Button
            component={Link}
            disabled={page >= query.data.totalPages}
            href={`/app/funcoes-cargos?page=${page + 1}`}
          >
            Próxima
          </Button>
        </Stack>
      ) : null}
    </Stack>
  );
}
