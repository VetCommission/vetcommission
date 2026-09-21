"use client";

import { CircularProgress, Container } from "@mui/material";
import { useParams } from "next/navigation";
import { useQuery } from "@tanstack/react-query";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { useTenant } from "@/features/auth/TenantProvider";
import { getProfessional } from "@/features/professionals/professionalsApi";
import { professionalQueryKeys } from "@/features/professionals/professionalQueryKeys";
import { ProfessionalForm } from "@/features/professionals/ProfessionalForm";

export default function ProfessionalDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const { activeTenantId } = useTenant();
  const query = useQuery({
    queryKey: activeTenantId
      ? professionalQueryKeys.detail(activeTenantId, id)
      : ["tenantless", "professional", id],
    queryFn: ({ signal }) => getProfessional(id, signal),
    enabled: Boolean(activeTenantId),
  });

  return (
    <RequireAcesso recurso={accessResources.professionalsManage}>
      <Container maxWidth="md" sx={{ py: { xs: 3, md: 5 } }}>
        {query.isLoading ? (
          <CircularProgress />
        ) : query.data ? (
          <ProfessionalForm initialMode="view" professional={query.data} />
        ) : null}
      </Container>
    </RequireAcesso>
  );
}
