import { Container } from "@mui/material";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalRolesList } from "@/features/professional-roles/ProfessionalRolesList";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
export default function ProfessionalRolesPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalRolesManage}>
      <AuthenticatedNavbar />
      <Container maxWidth="lg" sx={{ py: { xs: 3, md: 5 } }}>
        <ProfessionalRolesList />
      </Container>
    </RequireAcesso>
  );
}
