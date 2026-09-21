"use client";
import { Container } from "@mui/material";
import { RequireAcesso } from "@/features/auth/RequireAcesso";
import { accessResources } from "@/features/auth/accessResources";
import { ProfessionalSpecialtiesList } from "@/features/professional-specialties/ProfessionalSpecialtiesList";
export default function ProfessionalSpecialtiesPage() { return <RequireAcesso recurso={accessResources.professionalSpecialtiesManage}><Container maxWidth="lg" sx={{ py: { xs: 3, md: 5 } }}><ProfessionalSpecialtiesList /></Container></RequireAcesso>; }
