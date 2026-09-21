"use client";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button, Paper, Stack, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { saveProfessionalRole, saveProfessionalSpecialty } from "@/features/professionals/professionalsApi";
const schema = z.object({ name: z.string().trim().min(1, "Informe o nome.").max(120) });
export function MasterDataForm({ kind }: { kind: "role" | "specialty" }) { const router=useRouter(); const {register,handleSubmit}=useForm<{name:string}>({resolver:zodResolver(schema)}); const submit=async(data:{name:string})=>{await (kind==="role"?saveProfessionalRole(data.name):saveProfessionalSpecialty(data.name));router.replace(kind==="role"?"/app/funcoes-cargos":"/app/especialidades");}; return <Paper variant="outlined" sx={{p:4}}><Stack component="form" spacing={2} onSubmit={handleSubmit(submit)}><Typography variant="h4">{kind==="role"?"Nova função ou cargo":"Nova especialidade"}</Typography><TextField label="Nome" {...register("name")} /><Stack direction="row" spacing={2} sx={{justifyContent:"flex-end"}}><Button type="button" onClick={()=>router.back()}>Voltar</Button><Button type="submit" variant="contained">Salvar</Button></Stack></Stack></Paper>; }
