"use client";

import CheckCircleRoundedIcon from "@mui/icons-material/CheckCircleRounded";
import { Box, Button, Chip, Container, Paper, Stack, Typography } from "@mui/material";
import Image from "next/image";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useAuth } from "./AuthProvider";
import { AuthLoadingState } from "./AuthStates";
import { useTenant } from "./TenantProvider";
import { getAuthRedirectPath } from "./getAuthRedirectPath";

export function HomeRedirect() {
  const router = useRouter();
  const { isAuthenticated, isLoading } = useAuth();
  const { activeTenant } = useTenant();

  useEffect(() => {
    if (isAuthenticated && activeTenant) {
      router.replace(getAuthRedirectPath(activeTenant));
    }
  }, [activeTenant, isAuthenticated, router]);

  if (isLoading) {
    return <AuthLoadingState />;
  }

  return (
    <Container component="main" maxWidth="sm" sx={{ py: { xs: 6, md: 10 } }}>
      <Paper variant="outlined" sx={{ p: { xs: 3, md: 5 } }}>
        <Stack spacing={3}>
          <Box>
            <Typography component="p" color="primary" gutterBottom sx={{ fontWeight: 700 }}>
              <Image
                alt="VetCommission"
                height={48}
                src="/assets/brand/vetcom-logo.svg"
                width={190}
              />
            </Typography>
            <Typography color="text.secondary" variant="overline">
              Landing page institucional
            </Typography>
            <Typography component="h1" variant="h3">
              Fundação da aplicação pronta
            </Typography>
          </Box>

          <Typography color="text.secondary">
            Estrutura técnica inicial para a API, o Worker e o portal web. Entre com um usuário
            previamente cadastrado para acessar o MVP.
          </Typography>

          <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
            <Button component={Link} href="/login" variant="contained">
              Entrar
            </Button>
            <Chip
              color="success"
              icon={<CheckCircleRoundedIcon />}
              label="Landing page inicial"
              sx={{ alignSelf: { xs: "flex-start", sm: "center" } }}
            />
          </Stack>
        </Stack>
      </Paper>
    </Container>
  );
}
