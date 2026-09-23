import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalRolesList } from "@/features/professional-roles/ProfessionalRolesList";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
import { CrudPageContainer } from "@/components/layout/CrudPageContainer";
export default function ProfessionalRolesPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalRolesManage}>
      <AuthenticatedNavbar />
      <CrudPageContainer>
        <ProfessionalRolesList />
      </CrudPageContainer>
    </RequireAcesso>
  );
}
