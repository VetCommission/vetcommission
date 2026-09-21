"use client";

import AddRoundedIcon from "@mui/icons-material/AddRounded";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Alert, Avatar, Box, Button, Chip, CircularProgress, MenuItem, Paper, Select, Stack, Table, TableBody, TableCell, TableHead, TableRow, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { ApiError } from "@/lib/api/apiError";
import { listProfessionals, setProfessionalActive } from "./professionalsApi";
import { professionalQueryKeys } from "./professionalQueryKeys";

export function ProfessionalsList() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [search, setSearch] = useState("");
  const [role, setRole] = useState("");
  const [active, setActive] = useState("true");
  const filters = { search, role, active: active === "" ? undefined : active === "true" };
  const query = useQuery({ queryKey: professionalQueryKeys.list(filters), queryFn: () => listProfessionals(filters) });
  const statusMutation = useMutation({
    mutationFn: ({ id, value }: { id: string; value: boolean }) => setProfessionalActive(id, value),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: professionalQueryKeys.all }),
  });

  return (
    <Stack spacing={3}>
      <Stack direction={{ xs: "column", sm: "row" }} spacing={2} sx={{ justifyContent: "space-between" }}>
        <Box><Typography component="h1" variant="h4">Profissionais</Typography><Typography color="text.secondary">Gerencie os profissionais da clínica.</Typography></Box>
        <Button startIcon={<AddRoundedIcon />} variant="contained" onClick={() => router.push("/app/profissionais/novo")}>Novo profissional</Button>
      </Stack>
      <Paper variant="outlined" sx={{ p: 2 }}>
        <Stack direction={{ xs: "column", md: "row" }} spacing={2}>
          <TextField fullWidth label="Buscar por nome" value={search} onChange={(event) => setSearch(event.target.value)} />
          <TextField fullWidth label="Função" value={role} onChange={(event) => setRole(event.target.value)} />
          <Select fullWidth value={active} onChange={(event) => setActive(event.target.value)} aria-label="Status"><MenuItem value="true">Ativos</MenuItem><MenuItem value="false">Inativos</MenuItem><MenuItem value="">Todos</MenuItem></Select>
        </Stack>
      </Paper>
      {query.isLoading ? <CircularProgress aria-label="Carregando profissionais" /> : null}
      {query.isError ? <Alert severity="error">{query.error instanceof ApiError ? query.error.message : "Não foi possível carregar profissionais."}</Alert> : null}
      {!query.isLoading && !query.isError && query.data?.length === 0 ? <Alert severity="info">Nenhum profissional encontrado.</Alert> : null}
      {query.data && query.data.length > 0 ? <Paper variant="outlined" sx={{ overflowX: "auto" }}><Table sx={{ minWidth: 760 }}><TableHead><TableRow><TableCell sx={{ width: 72 }} /><TableCell>Profissional</TableCell><TableCell>Função</TableCell><TableCell>Registro</TableCell><TableCell>Status</TableCell><TableCell align="right">Ações</TableCell></TableRow></TableHead><TableBody>{query.data.map((professional) => <TableRow key={professional.id} hover><TableCell><Avatar>{professional.name.charAt(0).toUpperCase()}</Avatar></TableCell><TableCell><Typography fontWeight={600}>{professional.name}</Typography>{professional.email ? <Typography variant="body2" color="text.secondary">{professional.email}</Typography> : null}</TableCell><TableCell>{professional.role}</TableCell><TableCell>{professional.professionalRegistration ?? "—"}</TableCell><TableCell><Chip color={professional.active ? "success" : "default"} label={professional.active ? "Ativo" : "Inativo"} size="small" /></TableCell><TableCell align="right"><Button onClick={() => router.push(`/app/profissionais/${professional.id}`)}>Visualizar</Button><Button onClick={() => statusMutation.mutate({ id: professional.id, value: !professional.active })}>{professional.active ? "Inativar" : "Ativar"}</Button></TableCell></TableRow>)}</TableBody></Table></Paper> : null}
    </Stack>
  );
}
