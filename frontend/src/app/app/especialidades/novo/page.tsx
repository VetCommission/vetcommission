import { CrudPageContainer } from "@/components/layout/CrudPageContainer";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { MasterDataForm } from "@/features/master-data/MasterDataForm";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";

export default function Page() {
  return (
    <RequireAcesso recurso={accessResources.professionalSpecialtiesManage}>
      <AuthenticatedNavbar />
      <CrudPageContainer>
        <MasterDataForm kind="specialty" />
      </CrudPageContainer>
    </RequireAcesso>
  );
}
