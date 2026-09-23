import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalForm } from "@/features/professionals/ProfessionalForm";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
import { CrudPageContainer } from "@/components/layout/CrudPageContainer";

export default function NewProfessionalPage() {
  return (
    <RequireAcesso recurso={accessResources.professionalsManage}>
      <AuthenticatedNavbar />
      <CrudPageContainer>
        <ProfessionalForm />
      </CrudPageContainer>
    </RequireAcesso>
  );
}
