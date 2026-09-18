"use client";

import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import ReportProblemOutlinedIcon from "@mui/icons-material/ReportProblemOutlined";
import { Alert, Box, Button, CircularProgress, Paper, Stack, Typography } from "@mui/material";
import Link from "next/link";

export function AuthLoadingState() {
  return (
    <Box sx={{ display: "grid", minHeight: "60vh", placeItems: "center", px: 2 }}>
      <Stack spacing={2} sx={{ alignItems: "center" }}>
        <CircularProgress aria-label="Carregando sessão" />
        <Typography color="text.secondary">Carregando sua sessão...</Typography>
      </Stack>
    </Box>
  );
}

export function UnauthorizedState() {
  return (
    <Box sx={{ display: "grid", minHeight: "60vh", placeItems: "center", px: 2 }}>
      <Paper variant="outlined" sx={{ maxWidth: 440, p: 4, textAlign: "center" }}>
        <Stack spacing={2} sx={{ alignItems: "center" }}>
          <LockOutlinedIcon color="primary" fontSize="large" />
          <Typography component="h1" variant="h5">
            Acesso protegido
          </Typography>
          <Typography color="text.secondary">
            Entre com um usuário previamente cadastrado para continuar.
          </Typography>
          <Button component={Link} href="/login" variant="contained">
            Ir para login
          </Button>
        </Stack>
      </Paper>
    </Box>
  );
}

export function ForbiddenState() {
  return (
    <Box sx={{ display: "grid", minHeight: "60vh", placeItems: "center", px: 2 }}>
      <Paper variant="outlined" sx={{ maxWidth: 520, p: 4 }}>
        <Stack spacing={2}>
          <ReportProblemOutlinedIcon color="warning" fontSize="large" />
          <Typography component="h1" variant="h5">
            Sem permissão para acessar esta área
          </Typography>
          <Typography color="text.secondary">
            Seu usuário não possui o recurso necessário no tenant selecionado.
          </Typography>
          <Alert severity="info">
            Se você acredita que deveria ter acesso, solicite a revisão do seu grupo de acesso.
          </Alert>
        </Stack>
      </Paper>
    </Box>
  );
}

export function MissingTenantState() {
  return (
    <Box sx={{ display: "grid", minHeight: "60vh", placeItems: "center", px: 2 }}>
      <Paper variant="outlined" sx={{ maxWidth: 520, p: 4 }}>
        <Stack spacing={2}>
          <Typography component="h1" variant="h5">
            Nenhum tenant ativo
          </Typography>
          <Typography color="text.secondary">
            Não encontramos um vínculo ativo de tenant para esta sessão.
          </Typography>
          <Alert severity="warning">
            O acesso ao sistema depende de um usuário ativo vinculado a pelo menos um tenant.
          </Alert>
        </Stack>
      </Paper>
    </Box>
  );
}
