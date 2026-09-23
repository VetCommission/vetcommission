"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { CrudPageContainer } from "@/components/layout/CrudPageContainer";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { MasterDataForm } from "@/features/master-data/MasterDataForm";
import { listProfessionalRoles } from "@/features/professional-roles/professionalRolesApi";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const query = useQuery({ queryKey: ["professional-role", id], queryFn: () => listProfessionalRoles(1, 100).then(result => result.items.find(item => item.id === id)) });
  return <RequireAcesso recurso={accessResources.professionalRolesManage}><AuthenticatedNavbar /><CrudPageContainer>{query.data ? <MasterDataForm kind="role" id={query.data.id} initialName={query.data.name} initialMode="view" /> : null}</CrudPageContainer></RequireAcesso>;
}
