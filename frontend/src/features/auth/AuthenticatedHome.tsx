"use client";

import LogoutRoundedIcon from "@mui/icons-material/LogoutRounded";
import PetsRoundedIcon from "@mui/icons-material/PetsRounded";
import {
  AppBar,
  Box,
  Button,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  Toolbar,
  Typography,
} from "@mui/material";
import { useRouter } from "next/navigation";
import { useAuth } from "./AuthProvider";
import { useTenant } from "./TenantProvider";

type AuthenticatedHomeProps = {
  area: "administrativa" | "do profissional";
};

export function AuthenticatedHome({ area }: AuthenticatedHomeProps) {
  const router = useRouter();
  const { logout, session } = useAuth();
  const { activeTenantId, activeTenant, selectTenant, tenants } = useTenant();

  function handleLogout() {
    logout();
    router.replace("/login");
  }

  return (
    <Box sx={{ minHeight: "100vh", bgcolor: "background.default" }}>
      <AppBar color="inherit" elevation={0} position="static">
        <Toolbar sx={{ gap: 2 }}>
          <PetsRoundedIcon color="primary" />
          <Box sx={{ flexGrow: 1 }}>
            <Typography sx={{ fontWeight: 700 }}>VetCommission</Typography>
            <Typography color="text.secondary" variant="caption">
              Área {area}
            </Typography>
          </Box>
          <Button color="inherit" onClick={handleLogout} startIcon={<LogoutRoundedIcon />}>
            Sair
          </Button>
        </Toolbar>
      </AppBar>

      <Container component="main" maxWidth="lg" sx={{ py: { xs: 3, md: 5 } }}>
        <Stack spacing={3}>
          <Paper variant="outlined" sx={{ p: { xs: 3, md: 4 } }}>
            <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
              <Box sx={{ flexGrow: 1 }}>
                <Typography component="h1" variant="h4">
                  Olá, {session?.usuario.nome}
                </Typography>
                <Typography color="text.secondary" sx={{ mt: 1 }}>
                  Você está acessando o tenant{" "}
                  <Typography component="span" color="text.primary" sx={{ fontWeight: 700 }}>
                    {activeTenant?.nome}
                  </Typography>
                  .
                </Typography>
              </Box>

              <FormControl sx={{ minWidth: { xs: "100%", md: 280 } }}>
                <InputLabel id="tenant-select-label">Tenant ativo</InputLabel>
                <Select
                  disabled={tenants.length <= 1}
                  label="Tenant ativo"
                  labelId="tenant-select-label"
                  value={activeTenantId ?? ""}
                  onChange={(event) => selectTenant(event.target.value)}
                >
                  {tenants.map((tenant) => (
                    <MenuItem key={tenant.tenantId} value={tenant.tenantId}>
                      {tenant.nome}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Stack>
          </Paper>

          <Paper variant="outlined" sx={{ p: { xs: 3, md: 4 } }}>
            <Stack spacing={1}>
              <Typography component="h2" variant="h5">
                Base autenticada pronta
              </Typography>
              <Typography color="text.secondary">
                Login, sessão, tenant ativo, permissões e proteção de rotas estão prontos para
                as próximas etapas do MVP.
              </Typography>
            </Stack>
          </Paper>
        </Stack>
      </Container>
    </Box>
  );
}
