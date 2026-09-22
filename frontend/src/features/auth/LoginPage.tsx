"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import LoginRoundedIcon from "@mui/icons-material/LoginRounded";
import { Alert, Box, Button, Container, Paper, Stack, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import Image from "next/image";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { ApiError } from "@/lib/api/apiError";
import { useAuth } from "./AuthProvider";
import { getAuthRedirectPath } from "./getAuthRedirectPath";

const loginSchema = z.object({
  email: z.string().trim().min(1, "Informe o e-mail.").email("Informe um e-mail válido."),
  senha: z.string().min(1, "Informe a senha."),
});

type LoginFormData = z.infer<typeof loginSchema>;

function getLoginErrorMessage(error: unknown) {
  if (error instanceof ApiError) {
    if (error.status === 401) {
      return "E-mail ou senha inválidos.";
    }

    if (error.status === 403) {
      return "Seu usuário não possui vínculo ativo com um tenant.";
    }

    return error.message;
  }

  return "Não foi possível entrar. Tente novamente.";
}

export function LoginPage() {
  const router = useRouter();
  const { login, isLoading } = useAuth();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const {
    formState: { errors, isSubmitting },
    handleSubmit,
    register,
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: process.env.NODE_ENV === "development" ? "admin@vetcommission.local" : "",
      senha: process.env.NODE_ENV === "development" ? "qwas" : "",
    },
  });

  async function onSubmit(data: LoginFormData) {
    setErrorMessage(null);

    try {
      const session = await login(data);
      router.replace(getAuthRedirectPath(session.tenants[0] ?? null));
    } catch (error) {
      setErrorMessage(getLoginErrorMessage(error));
    }
  }

  return (
    <Container component="main" maxWidth={false} disableGutters sx={{ minHeight: "100vh" }}>
      <Paper
        square
        variant="outlined"
        sx={{ border: 0, borderRadius: 0, minHeight: "100vh", overflow: "hidden" }}
      >
        <Stack direction={{ xs: "column", md: "row" }} sx={{ minHeight: "100vh" }}>
          <Box
            sx={{
              display: { xs: "none", md: "block" },
              minHeight: "100vh",
              position: "relative",
              borderRadius: 0,
              width: { md: "60%", lg: "65%" },
              "&::after": {
                background:
                  "linear-gradient(115deg, rgba(7, 45, 35, 0.82) 0%, rgba(19, 105, 79, 0.55) 48%, rgba(19, 105, 79, 0.08) 100%)",
                content: '""',
                inset: 0,
                position: "absolute",
              },
            }}
          >
            <Image
              alt="Profissional veterinária em uma clínica"
              fill
              priority
              sizes="65vw"
              src="/assets/images/login-veterinary.webp"
              style={{ borderRadius: 0, objectFit: "cover" }}
            />
          </Box>
          <Stack
            component="form"
            noValidate
            onSubmit={handleSubmit(onSubmit)}
            spacing={3}
            sx={{
              bgcolor: "background.paper",
              flex: 1,
              justifyContent: "center",
              maxWidth: { md: 560, lg: 620 },
              p: { xs: 3, sm: 5, lg: 8 },
              width: { md: "40%", lg: "35%" },
            }}
          >
            <Box>
              <Typography
                component="p"
                color="primary"
                gutterBottom
                sx={{ fontWeight: 700, mb: 4, textAlign: "center" }}
              >
                <Image
                  alt="VetCom"
                  height={64}
                  src="/assets/brand/vetcom-logo.svg"
                  width={255}
                />
              </Typography>
              <Typography component="h1" color="text.secondary" sx={{ mt: "10%" }}>
                Entre para continuar.
              </Typography>
            </Box>

            {errorMessage ? <Alert severity="error">{errorMessage}</Alert> : null}

            <TextField
              autoComplete="email"
              autoFocus
              error={Boolean(errors.email)}
              helperText={errors.email?.message}
              label="E-mail"
              type="email"
              sx={{ alignSelf: "center", width: "100%", maxWidth: 520 }}
              {...register("email")}
            />

            <TextField
              autoComplete="current-password"
              error={Boolean(errors.senha)}
              helperText={errors.senha?.message}
              label="Senha"
              type="password"
              sx={{ alignSelf: "center", width: "100%", maxWidth: 520 }}
              {...register("senha")}
            />

            <Button
              disabled={isSubmitting || isLoading}
              size="large"
              startIcon={<LoginRoundedIcon />}
              type="submit"
              variant="contained"
              sx={{ alignSelf: "center", width: "100%", maxWidth: 520 }}
            >
              Entrar
            </Button>
          </Stack>
        </Stack>
      </Paper>
    </Container>
  );
}
