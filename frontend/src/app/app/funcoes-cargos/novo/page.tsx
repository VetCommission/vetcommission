import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { MasterDataForm } from "@/features/master-data/MasterDataForm";
import { CrudPageContainer } from "@/components/layout/CrudPageContainer";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";

export default function Page() {
  return (
    <RequireAcesso recurso={accessResources.professionalRolesManage}>
      <AuthenticatedNavbar />
      <CrudPageContainer>
        <MasterDataForm kind="role" />
      </CrudPageContainer>
    </RequireAcesso>
  );
}
