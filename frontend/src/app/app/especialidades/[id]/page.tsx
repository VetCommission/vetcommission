"use client";
import { Container } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { listProfessionalSpecialties } from "@/features/professional-specialties/professionalSpecialtiesApi";
import { MasterDataForm } from "@/features/master-data/MasterDataForm";
export default function Page(){const {id}=useParams<{id:string}>();const q=useQuery({queryKey:["professional-specialty",id],queryFn:()=>listProfessionalSpecialties(1,100).then(x=>x.items.find(i=>i.id===id))});return <RequireAcesso recurso={accessResources.professionalSpecialtiesManage}><Container maxWidth="md" sx={{py:5}}>{q.data?<MasterDataForm kind="specialty" id={q.data.id} initialName={q.data.name} initialMode="view"/>:null}</Container></RequireAcesso>}
