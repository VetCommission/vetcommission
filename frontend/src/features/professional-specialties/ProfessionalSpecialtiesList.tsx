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
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { listProfessionalSpecialties } from "./professionalSpecialtiesApi";
import { useTenant } from "@/features/auth/TenantProvider";

export function ProfessionalSpecialtiesList() {
  const { activeTenantId } = useTenant();
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
      <Paper variant="outlined">
        <Table aria-label="Lista de especialidades">
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
