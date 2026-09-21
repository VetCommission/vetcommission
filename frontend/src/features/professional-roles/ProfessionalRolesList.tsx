"use client";

import { useQuery } from "@tanstack/react-query";
import {
  Alert,
  Button,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import { useRouter, useSearchParams } from "next/navigation";
import { listProfessionalRoles } from "./professionalRolesApi";
import { useTenant } from "@/features/auth/TenantProvider";

export function ProfessionalRolesList() {
  const router = useRouter();
  const { activeTenantId } = useTenant();
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
        <Button variant="contained" onClick={() => router.push("/app/funcoes-cargos/novo")}>
          Novo cadastro
        </Button>
      </Stack>
      {query.isError ? (
        <Alert severity="error">Não foi possível carregar funções e cargos.</Alert>
      ) : null}
      <Paper variant="outlined">
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Status</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {query.data?.items.map((item) => (
              <TableRow key={item.id} hover>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.active ? "Ativo" : "Inativo"}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
      {query.data ? (
        <Stack direction="row" sx={{ justifyContent: "flex-end" }}>
          <Button
            disabled={page <= 1}
            onClick={() => router.push(`/app/funcoes-cargos?page=${page - 1}`)}
          >
            Anterior
          </Button>
          <Button
            disabled={page >= query.data.totalPages}
            onClick={() => router.push(`/app/funcoes-cargos?page=${page + 1}`)}
          >
            Próxima
          </Button>
        </Stack>
      ) : null}
    </Stack>
  );
}
