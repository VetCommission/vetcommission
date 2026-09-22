"use client";

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
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
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { listProfessionalSpecialties, setProfessionalSpecialtyActive } from "./professionalSpecialtiesApi";
import { useTenant } from "@/features/auth/TenantProvider";

export function ProfessionalSpecialtiesList() {
  const { activeTenantId } = useTenant();
  const queryClient = useQueryClient();
  const statusMutation = useMutation({ mutationFn: ({ id, active }: { id: string; active: boolean }) => setProfessionalSpecialtyActive(id, active), onSuccess: () => queryClient.invalidateQueries({ queryKey: ["tenant", activeTenantId, "professional-specialties"] }) });
  const page = Math.max(1, Number(useSearchParams().get("page") ?? "1") || 1);
  const query = useQuery({
    queryKey: ["tenant", activeTenantId, "professional-specialties", "list", page],
    queryFn: ({ signal }) => listProfessionalSpecialties(page, 20, signal),
    enabled: Boolean(activeTenantId),
  });
  return (
    <Stack spacing={3}>
      <Stack direction="row" sx={{ justifyContent: "space-between", alignItems: "center" }}>
        <Typography component="h1" variant="h4">
          Especialidades
        </Typography>
        <Button component={Link} href="/app/especialidades/novo" variant="contained">
          Novo cadastro
        </Button>
      </Stack>
      {query.isError ? (
        <Alert severity="error">Não foi possível carregar especialidades.</Alert>
      ) : null}
      <Paper variant="outlined" sx={{ width: "100%", overflowX: "auto" }}>
        <Table aria-label="Lista de especialidades">
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="right">Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {query.data?.items.map((item) => (
              <TableRow key={item.id} hover>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.active ? "Ativo" : "Inativo"}</TableCell>
                <TableCell align="right"><Button component={Link} href={`/app/especialidades/${item.id}`}>Visualizar</Button><Button disabled={statusMutation.isPending} onClick={() => statusMutation.mutate({ id: item.id, active: !item.active })}>{item.active ? "Inativar" : "Ativar"}</Button></TableCell>
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
            href={`/app/especialidades?page=${page - 1}`}
          >
            Anterior
          </Button>
          <Button
            component={Link}
            disabled={page >= query.data.totalPages}
            href={`/app/especialidades?page=${page + 1}`}
          >
            Próxima
          </Button>
        </Stack>
      ) : null}
    </Stack>
  );
}
