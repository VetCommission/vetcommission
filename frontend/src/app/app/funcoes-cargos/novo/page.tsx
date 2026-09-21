import { Container } from "@mui/material";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { MasterDataForm } from "@/features/master-data/MasterDataForm";

export default function Page() {
  return (
    <RequireAcesso recurso={accessResources.professionalRolesManage}>
      <Container maxWidth="md" sx={{ py: 5 }}>
        <MasterDataForm kind="role" />
      </Container>
    </RequireAcesso>
  );
}
