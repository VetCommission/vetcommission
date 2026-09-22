import { Container } from "@mui/material";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalsList } from "@/features/professionals/ProfessionalsList";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";

export default function ProfessionalsPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalsManage}>
      <AuthenticatedNavbar />
      <Container maxWidth="lg" sx={{ py: { xs: 3, md: 5 } }}>
        <ProfessionalsList />
      </Container>
    </RequireAcesso>
  );
}
