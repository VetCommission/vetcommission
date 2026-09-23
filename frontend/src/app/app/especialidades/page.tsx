import { CrudPageContainer } from "@/components/layout/CrudPageContainer";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalSpecialtiesList } from "@/features/professional-specialties/ProfessionalSpecialtiesList";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
export default function ProfessionalSpecialtiesPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalSpecialtiesManage}>
      <AuthenticatedNavbar />
      <CrudPageContainer>
        <ProfessionalSpecialtiesList />
      </CrudPageContainer>
    </RequireAcesso>
  );
}
