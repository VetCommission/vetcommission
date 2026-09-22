"use client";

import { Alert, Button, CircularProgress, Container, Stack } from "@mui/material";
import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { useTenant } from "@/features/auth/TenantProvider";
import { ApiError } from "@/lib/api/apiError";
import { getProfessional } from "./professionalsApi";
import { professionalQueryKeys } from "./professionalQueryKeys";
import { ProfessionalForm } from "./ProfessionalForm";

type ProfessionalDetailsClientProps = {
  id: string;
};

export function ProfessionalDetailsClient({ id }: ProfessionalDetailsClientProps) {
  const { activeTenantId } = useTenant();
  const query = useQuery({
    queryKey: activeTenantId
      ? professionalQueryKeys.detail(activeTenantId, id)
      : ["tenantless", "professional", id],
    queryFn: ({ signal }) => getProfessional(id, signal),
    enabled: Boolean(activeTenantId),
  });

  if (query.isError && !(query.error instanceof ApiError && query.error.status === 404)) {
    throw query.error;
  }

  return (
    <RequireAcesso recurso={accessResources.professionalsManage}>
      <Container maxWidth="md" sx={{ py: { xs: 3, md: 5 } }}>
        {query.isLoading ? <CircularProgress aria-label="Carregando profissional" /> : null}
        {query.isError ? (
          <Stack spacing={2}>
            <Alert
              severity={
                query.error instanceof ApiError && query.error.status === 404 ? "info" : "error"
              }
            >
              {query.error instanceof ApiError && query.error.status === 404
                ? "Profissional não encontrado ou não pertence ao tenant atual."
                : query.error instanceof ApiError
                  ? query.error.message
                  : "Não foi possível carregar o profissional."}
            </Alert>
            <Button component={Link} href="/app/profissionais" variant="outlined">
              Voltar para profissionais
            </Button>
          </Stack>
        ) : null}
        {query.data ? <ProfessionalForm initialMode="view" professional={query.data} /> : null}
      </Container>
    </RequireAcesso>
  );
}
