import { AuthenticatedHome } from "@/features/auth/AuthenticatedHome";
import { accessResources } from "@/features/auth/accessResources";
import { RequireAcesso } from "@/features/auth/RequireAcesso";

export default function Page() {
  return (
    <RequireAcesso recurso={accessResources.professionalPortal}>
      <AuthenticatedHome area="do profissional" />
    </RequireAcesso>
  );
}
