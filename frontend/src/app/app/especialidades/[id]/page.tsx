"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { CrudPageContainer } from "@/components/layout/CrudPageContainer";
import { AuthenticatedNavbar } from "@/features/auth/AuthenticatedNavbar";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { MasterDataForm } from "@/features/master-data/MasterDataForm";
import { listProfessionalSpecialties } from "@/features/professional-specialties/professionalSpecialtiesApi";

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const query = useQuery({ queryKey: ["professional-specialty", id], queryFn: () => listProfessionalSpecialties(1, 100).then(result => result.items.find(item => item.id === id)) });
  return <RequireAcesso recurso={accessResources.professionalSpecialtiesManage}><AuthenticatedNavbar /><CrudPageContainer>{query.data ? <MasterDataForm kind="specialty" id={query.data.id} initialName={query.data.name} initialMode="view" /> : null}</CrudPageContainer></RequireAcesso>;
}
