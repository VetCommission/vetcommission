"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import LoginRoundedIcon from "@mui/icons-material/LoginRounded";
import {
  Alert,
  Box,
  Button,
  Container,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { useRouter } from "next/navigation";
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
      email: "",
      senha: "",
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
    <Container component="main" maxWidth="sm" sx={{ py: { xs: 5, md: 9 } }}>
      <Paper variant="outlined" sx={{ p: { xs: 3, md: 5 } }}>
        <Stack component="form" noValidate spacing={3} onSubmit={handleSubmit(onSubmit)}>
          <Box>
            <Typography component="p" color="primary" gutterBottom sx={{ fontWeight: 700 }}>
              VetCommission
            </Typography>
            <Typography component="h1" variant="h4">
              Entrar no sistema
            </Typography>
            <Typography color="text.secondary" sx={{ mt: 1 }}>
              Acesso exclusivo para usuários previamente cadastrados pela clínica.
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
            {...register("email")}
          />

          <TextField
            autoComplete="current-password"
            error={Boolean(errors.senha)}
            helperText={errors.senha?.message}
            label="Senha"
            type="password"
            {...register("senha")}
          />

          <Button
            disabled={isSubmitting || isLoading}
            size="large"
            startIcon={<LoginRoundedIcon />}
            type="submit"
            variant="contained"
          >
            Entrar
          </Button>

          <Alert severity="info">
            O MVP não possui autocadastro público nem recuperação automatizada de senha.
          </Alert>
        </Stack>
      </Paper>
    </Container>
  );
}
