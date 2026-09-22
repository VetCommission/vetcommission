import { Container } from "@mui/material";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalForm } from "@/features/professionals/ProfessionalForm";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";

export default function NewProfessionalPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalsManage}>
      <AuthenticatedNavbar />
      <Container maxWidth="md" sx={{ py: { xs: 3, md: 5 } }}>
        <ProfessionalForm />
      </Container>
    </RequireAcesso>
  );
}
