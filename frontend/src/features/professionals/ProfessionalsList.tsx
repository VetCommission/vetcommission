"use client";

import AddRoundedIcon from "@mui/icons-material/AddRounded";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  Alert,
  Avatar,
  Box,
  Button,
  Chip,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  MenuItem,
  Paper,
  Select,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from "@mui/material";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useTenant } from "@/features/auth/TenantProvider";
import { ApiError } from "@/lib/api/apiError";
import { listProfessionals, setProfessionalActive } from "./professionalsApi";
import { professionalQueryKeys } from "./professionalQueryKeys";

const pageSize = 20;

export function ProfessionalsList() {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const queryClient = useQueryClient();
  const { activeTenantId } = useTenant();
  const [search, setSearch] = useState(searchParams.get("search") ?? "");
  const [role, setRole] = useState(searchParams.get("role") ?? "");
  const [active, setActive] = useState(searchParams.get("active") ?? "true");
  const [debouncedSearch, setDebouncedSearch] = useState(search);
  const [pendingAction, setPendingAction] = useState<{
    id: string;
    name: string;
    active: boolean;
  } | null>(null);
  const page = Math.max(1, Number(searchParams.get("page") ?? "1") || 1);

  useEffect(() => {
    const timeout = window.setTimeout(() => setDebouncedSearch(search), 300);
    return () => window.clearTimeout(timeout);
  }, [search]);

  const updateUrl = useCallback(
    (changes: Record<string, string | undefined>) => {
      const next = new URLSearchParams(searchParams.toString());
      Object.entries(changes).forEach(([key, value]) =>
        value ? next.set(key, value) : next.delete(key),
      );
      router.replace(`${pathname}?${next.toString()}`);
    },
    [pathname, router, searchParams],
  );

  useEffect(() => {
    updateUrl({
      search: debouncedSearch || undefined,
      role: role || undefined,
      active: active || undefined,
      page: "1",
    });
  }, [active, debouncedSearch, role, updateUrl]);

  const filters = useMemo(
    () => ({
      page,
      pageSize,
      search: searchParams.get("search") ?? undefined,
      role: searchParams.get("role") ?? undefined,
      active: searchParams.get("active") === null ? true : searchParams.get("active") === "true",
    }),
    [page, searchParams],
  );
  const query = useQuery({
    queryKey: activeTenantId
      ? professionalQueryKeys.list(activeTenantId, filters)
      : ["tenantless", "professionals"],
    queryFn: ({ signal }) => listProfessionals(filters, signal),
    enabled: Boolean(activeTenantId),
  });
  const statusMutation = useMutation({
    mutationFn: ({ id, value }: { id: string; value: boolean }) => setProfessionalActive(id, value),
    onSuccess: () =>
      activeTenantId &&
      queryClient.invalidateQueries({ queryKey: professionalQueryKeys.all(activeTenantId) }),
  });
  const data = query.data;

  const confirmAction = () => {
    if (pendingAction) {
      statusMutation.mutate(
        { id: pendingAction.id, value: !pendingAction.active },
        { onSettled: () => setPendingAction(null) },
      );
    }
  };

  return (
    <Stack spacing={3}>
      <Stack
        direction={{ xs: "column", sm: "row" }}
        spacing={2}
        sx={{ justifyContent: "space-between" }}
      >
        <Box>
          <Typography component="h1" variant="h4">
            Profissionais
          </Typography>
          <Typography color="text.secondary">Gerencie os profissionais da clínica.</Typography>
        </Box>
        <Button
          startIcon={<AddRoundedIcon />}
          variant="contained"
          onClick={() => router.push("/app/profissionais/novo")}
        >
          Novo profissional
        </Button>
      </Stack>
      <Paper variant="outlined" sx={{ p: 2 }}>
        <Stack direction={{ xs: "column", md: "row" }} spacing={2}>
          <TextField
            fullWidth
            label="Buscar por nome"
            value={search}
            onChange={(event) => setSearch(event.target.value)}
          />
          <TextField
            fullWidth
            label="Função"
            value={role}
            onChange={(event) => {
              setRole(event.target.value);
              updateUrl({ role: event.target.value || undefined, page: "1" });
            }}
          />
          <Select
            fullWidth
            value={active}
            onChange={(event) => {
              setActive(event.target.value);
              updateUrl({ active: event.target.value || undefined, page: "1" });
            }}
            aria-label="Status"
          >
            <MenuItem value="true">Ativos</MenuItem>
            <MenuItem value="false">Inativos</MenuItem>
            <MenuItem value="">Todos</MenuItem>
          </Select>
        </Stack>
      </Paper>
      {query.isLoading ? <CircularProgress aria-label="Carregando profissionais" /> : null}
      {query.isError ? (
        <Alert severity="error">
          {query.error instanceof ApiError
            ? query.error.message
            : "Não foi possível carregar profissionais."}
        </Alert>
      ) : null}
      {!query.isLoading && !query.isError && data?.items.length === 0 ? (
        <Alert severity="info">Nenhum profissional encontrado.</Alert>
      ) : null}
      {data && data.items.length > 0 ? (
        <>
          <Paper variant="outlined" sx={{ overflowX: "auto" }}>
            <Table sx={{ minWidth: 760 }}>
              <TableHead>
                <TableRow>
                  <TableCell sx={{ width: 72 }} />
                  <TableCell>Profissional</TableCell>
                  <TableCell>Função</TableCell>
                  <TableCell>Registro</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell align="right">Ações</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {data.items.map((professional) => (
                  <TableRow key={professional.id} hover>
                    <TableCell>
                      <Avatar>{professional.name.charAt(0).toUpperCase()}</Avatar>
                    </TableCell>
                    <TableCell>
                      <Typography sx={{ fontWeight: 600 }}>{professional.name}</Typography>
                      {professional.email ? (
                        <Typography variant="body2" color="text.secondary">
                          {professional.email}
                        </Typography>
                      ) : null}
                    </TableCell>
                    <TableCell>{professional.role}</TableCell>
                    <TableCell>{professional.professionalRegistration ?? "—"}</TableCell>
                    <TableCell>
                      <Chip
                        color={professional.active ? "success" : "default"}
                        label={professional.active ? "Ativo" : "Inativo"}
                        size="small"
                      />
                    </TableCell>
                    <TableCell align="right">
                      <Button onClick={() => router.push(`/app/profissionais/${professional.id}`)}>
                        Visualizar
                      </Button>
                      <Button
                        disabled={statusMutation.isPending}
                        onClick={() =>
                          setPendingAction({
                            id: professional.id,
                            name: professional.name,
                            active: professional.active,
                          })
                        }
                      >
                        {professional.active ? "Inativar" : "Ativar"}
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </Paper>
          <Stack
            direction="row"
            spacing={2}
            sx={{ justifyContent: "flex-end", alignItems: "center" }}
          >
            <Typography variant="body2">
              Página {data.page} de {data.totalPages || 1}
            </Typography>
            <Button disabled={page <= 1} onClick={() => updateUrl({ page: String(page - 1) })}>
              Anterior
            </Button>
            <Button
              disabled={page >= data.totalPages}
              onClick={() => updateUrl({ page: String(page + 1) })}
            >
              Próxima
            </Button>
          </Stack>
        </>
      ) : null}
      <Dialog
        open={Boolean(pendingAction)}
        onClose={() => setPendingAction(null)}
        aria-labelledby="status-dialog-title"
      >
        <DialogTitle id="status-dialog-title">Confirmar alteração de status</DialogTitle>
        <DialogContent>
          <DialogContentText>
            {pendingAction?.active
              ? `Deseja inativar ${pendingAction.name}?`
              : `Deseja ativar ${pendingAction?.name}?`}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setPendingAction(null)}>Cancelar</Button>
          <Button variant="contained" onClick={confirmAction} disabled={statusMutation.isPending}>
            {statusMutation.isPending ? "Processando…" : "Confirmar"}
          </Button>
        </DialogActions>
      </Dialog>
    </Stack>
  );
}
