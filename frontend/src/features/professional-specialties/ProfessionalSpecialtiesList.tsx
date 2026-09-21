"use client";

import { useQuery } from "@tanstack/react-query";
import { Alert, Button, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { listProfessionalSpecialties } from "@/features/professionals/professionalsApi";

export function ProfessionalSpecialtiesList() {
  const router = useRouter();
  const query = useQuery({ queryKey: ["professional-specialties", "list"], queryFn: listProfessionalSpecialties });
  return <Stack spacing={3}>
    <Stack direction="row" sx={{ justifyContent: "space-between", alignItems: "center" }}><Typography component="h1" variant="h4">Especialidades</Typography><Button variant="contained" onClick={() => router.push("/app/especialidades/novo")}>Novo cadastro</Button></Stack>
    {query.isError ? <Alert severity="error">Não foi possível carregar especialidades.</Alert> : null}
    <Paper variant="outlined"><Table><TableHead><TableRow><TableCell>Nome</TableCell><TableCell>Status</TableCell><TableCell align="right">Ações</TableCell></TableRow></TableHead><TableBody>{query.data?.map((item) => <TableRow key={item.id} hover><TableCell>{item.name}</TableCell><TableCell>{item.active ? "Ativo" : "Inativo"}</TableCell><TableCell align="right"><Button onClick={() => router.push(`/app/especialidades/${item.id}`)}>Visualizar</Button></TableCell></TableRow>)}</TableBody></Table></Paper>
  </Stack>;
}
