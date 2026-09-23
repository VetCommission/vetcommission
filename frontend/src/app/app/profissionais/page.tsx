import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalsList } from "@/features/professionals/ProfessionalsList";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
import { CrudPageContainer } from "@/components/layout/CrudPageContainer";

export default function ProfessionalsPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalsManage}>
      <AuthenticatedNavbar />
      <CrudPageContainer>
        <ProfessionalsList />
      </CrudPageContainer>
    </RequireAcesso>
  );
}
