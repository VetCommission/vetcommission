"use client";

import ArrowForwardRoundedIcon from "@mui/icons-material/ArrowForwardRounded";
import CheckCircleRoundedIcon from "@mui/icons-material/CheckCircleRounded";
import GroupsRoundedIcon from "@mui/icons-material/GroupsRounded";
import PaymentsRoundedIcon from "@mui/icons-material/PaymentsRounded";
import TaskAltRoundedIcon from "@mui/icons-material/TaskAltRounded";
import { Box, Button, Chip, Container, Divider, Paper, Stack, Typography } from "@mui/material";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useAuth } from "./AuthProvider";
import { AuthLoadingState } from "./AuthStates";
import { useTenant } from "./TenantProvider";
import { getAuthRedirectPath } from "./getAuthRedirectPath";

export function HomeRedirect() {
  const router = useRouter();
  const { isAuthenticated, isLoading } = useAuth();
  const { activeTenant } = useTenant();
  useEffect(() => { if (isAuthenticated && activeTenant) router.replace(getAuthRedirectPath(activeTenant)); }, [activeTenant, isAuthenticated, router]);
  if (isLoading) return <AuthLoadingState />;

  return <Box sx={{ minHeight: "100vh", bgcolor: "#f7fafc", color: "#102a43" }}>
    <Box component="header" sx={{ position: "relative", zIndex: 1, borderBottom: "1px solid rgba(16,42,67,.08)", bgcolor: "rgba(255,255,255,.86)", backdropFilter: "blur(14px)" }}>
      <Container maxWidth="xl"><Stack direction="row" sx={{ minHeight: 76, alignItems: "center", justifyContent: "space-between" }}><Image alt="VetCom" height={42} src="/assets/brand/vetcom-logo.svg" width={168}/><Button href="/login" variant="contained" sx={{ borderRadius: 2, px: 2.5 }} endIcon={<ArrowForwardRoundedIcon/>}>Entrar no sistema</Button></Stack></Container>
    </Box>
    <Box sx={{ background: "linear-gradient(135deg,#eaf4ff 0%,#fff 58%,#eef8f1 100%)", borderBottom: "1px solid rgba(16,42,67,.06)" }}>
      <Container maxWidth="xl" sx={{ py: { xs: 7, md: 11 } }}><Box sx={{ display: "grid", gridTemplateColumns: { xs: "1fr", md: "1.05fr .95fr" }, gap: { xs: 6, md: 10 }, alignItems: "center" }}>
        <Stack spacing={3}><Chip icon={<CheckCircleRoundedIcon/>} color="primary" label="Gestão operacional para clínicas veterinárias" sx={{ alignSelf: "flex-start", fontWeight: 600 }}/><Typography component="h1" sx={{ maxWidth: 760, fontSize: { xs: "2.8rem", md: "4.7rem" }, lineHeight: 1.04, letterSpacing: "-.05em", fontWeight: 800 }}>Organize o trabalho da clínica e torne as comissões mais claras.</Typography><Typography color="text.secondary" sx={{ maxWidth: 650, fontSize: { xs: "1.1rem", md: "1.25rem" }, lineHeight: 1.7 }}>O VetCom ajuda clínicas veterinárias a cadastrar profissionais, registrar procedimentos e acompanhar o cálculo das comissões em um só lugar.</Typography><Stack direction={{ xs: "column", sm: "row" }} spacing={2}><Button href="/login" variant="contained" size="large" sx={{ alignSelf: "flex-start", borderRadius: 2, px: 3, boxShadow: "0 10px 24px rgba(25,118,210,.24)" }} endIcon={<ArrowForwardRoundedIcon/>}>Acessar minha conta</Button><Typography color="text.secondary" variant="body2" sx={{ alignSelf: "center" }}>Acesso seguro para usuários cadastrados.</Typography></Stack></Stack>
        <Paper elevation={0} sx={{ p: { xs: 2, md: 3 }, borderRadius: 4, border: "1px solid rgba(25,118,210,.16)", bgcolor: "rgba(255,255,255,.76)", boxShadow: "0 28px 70px rgba(16,42,67,.13)" }}><Box sx={{ bgcolor: "#102a43", color: "white", borderRadius: 3, p: { xs: 2.5, md: 3.5 } }}><Stack spacing={3}><Stack direction="row" sx={{ justifyContent: "space-between", alignItems: "center" }}><Typography sx={{ fontWeight: 700 }}>Resumo da operação</Typography><Chip size="small" label="Visão geral" sx={{ color: "#b9f6ca", bgcolor: "rgba(185,246,202,.14)" }}/></Stack><Typography sx={{ color: "rgba(255,255,255,.7)" }}>Uma visão simples para a clínica acompanhar o que está acontecendo.</Typography><Box sx={{ display: "grid", gridTemplateColumns: "repeat(2,1fr)", gap: 2 }}><Box sx={{ p: 2, bgcolor: "rgba(255,255,255,.08)", borderRadius: 2 }}><GroupsRoundedIcon color="info"/><Typography variant="h5" sx={{ mt: 1, fontWeight: 800 }}>24</Typography><Typography variant="body2" sx={{ opacity: .7 }}>profissionais</Typography></Box><Box sx={{ p: 2, bgcolor: "rgba(255,255,255,.08)", borderRadius: 2 }}><PaymentsRoundedIcon color="success"/><Typography variant="h5" sx={{ mt: 1, fontWeight: 800 }}>R$ 18,4k</Typography><Typography variant="body2" sx={{ opacity: .7 }}>comissões</Typography></Box></Box><Box sx={{ p: 2, bgcolor: "rgba(255,255,255,.08)", borderRadius: 2 }}><Stack direction="row" spacing={1} sx={{ alignItems: "center" }}><TaskAltRoundedIcon color="success"/><Typography>Dados organizados por competência</Typography></Stack></Box></Stack></Box></Paper>
      </Box></Container>
    </Box>
    <Container maxWidth="xl" sx={{ py: { xs: 7, md: 10 } }}><Stack spacing={8}>
      <Box><Typography color="primary" sx={{ fontWeight: 700, letterSpacing: ".08em", textTransform: "uppercase" }}>Como o VetCom ajuda</Typography><Typography component="h2" sx={{ mt: 1, maxWidth: 700, fontSize: { xs: "2rem", md: "3rem" }, fontWeight: 800, letterSpacing: "-.03em" }}>Menos planilhas dispersas. Mais clareza para decidir.</Typography><Typography color="text.secondary" sx={{ mt: 2, maxWidth: 680, fontSize: "1.1rem", lineHeight: 1.7 }}>A plataforma organiza as informações essenciais da operação para que a clínica e seus profissionais trabalhem com o mesmo entendimento.</Typography></Box>
      <Box sx={{ display: "grid", gridTemplateColumns: { xs: "1fr", md: "repeat(3,1fr)" }, gap: 3 }}><Paper variant="outlined" sx={{ p: 3.5, bgcolor: "white" }}><Typography color="primary" variant="h4" sx={{ fontWeight: 800 }}>01</Typography><Typography variant="h6" sx={{ mt: 2, fontWeight: 700 }}>Cadastre a equipe</Typography><Typography color="text.secondary" sx={{ mt: 1, lineHeight: 1.6 }}>Mantenha profissionais, funções e especialidades padronizados para evitar erros e retrabalho.</Typography></Paper><Paper variant="outlined" sx={{ p: 3.5, bgcolor: "white" }}><Typography color="primary" variant="h4" sx={{ fontWeight: 800 }}>02</Typography><Typography variant="h6" sx={{ mt: 2, fontWeight: 700 }}>Registre a produção</Typography><Typography color="text.secondary" sx={{ mt: 1, lineHeight: 1.6 }}>Centralize os procedimentos realizados e as informações necessárias para a conferência.</Typography></Paper><Paper variant="outlined" sx={{ p: 3.5, bgcolor: "white" }}><Typography color="primary" variant="h4" sx={{ fontWeight: 800 }}>03</Typography><Typography variant="h6" sx={{ mt: 2, fontWeight: 700 }}>Acompanhe as comissões</Typography><Typography color="text.secondary" sx={{ mt: 1, lineHeight: 1.6 }}>Tenha uma base organizada para consultar, conferir e evoluir os fechamentos da clínica.</Typography></Paper></Box>
      <Divider/><Stack direction={{ xs: "column", md: "row" }} spacing={3} sx={{ justifyContent: "space-between", alignItems: { md: "center" } }}><Box><Typography variant="h5" sx={{ fontWeight: 800 }}>Pronto para acessar o VetCom?</Typography><Typography color="text.secondary" sx={{ mt: 1 }}>Entre com um usuário previamente cadastrado pela clínica.</Typography></Box><Button href="/login" variant="contained" size="large" sx={{ alignSelf: { xs: "flex-start", md: "center" }, borderRadius: 2 }} endIcon={<ArrowForwardRoundedIcon/>}>Entrar no sistema</Button></Stack></Stack></Container>
    <Box component="footer" sx={{ py: 3, borderTop: "1px solid rgba(16,42,67,.08)", bgcolor: "white" }}><Container maxWidth="xl"><Typography color="text.secondary" variant="body2">VetCom · Gestão operacional para clínicas veterinárias</Typography></Container></Box>
  </Box>;
}
