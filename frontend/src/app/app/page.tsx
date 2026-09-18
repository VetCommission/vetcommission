import { accessResources } from "@/features/auth/accessResources";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { AuthenticatedHome } from "@/features/auth/AuthenticatedHome";

export default function Page() {
  return (
    <RequireAcesso recurso={accessResources.adminDashboard}>
      <AuthenticatedHome area="administrativa" />
    </RequireAcesso>
  );
}
