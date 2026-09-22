"use client";
import HomeRoundedIcon from "@mui/icons-material/HomeRounded";
import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import Image from "next/image";
import { useRouter } from "next/navigation";
export function AuthenticatedNavbar(){const router=useRouter();return <AppBar color="inherit" elevation={0} position="static"><Toolbar sx={{gap:2}}><Image alt="VetCommission" height={34} src="/assets/brand/vetcom-mark.svg" width={34}/><Box sx={{flexGrow:1}}><Typography sx={{fontWeight:700}}>VetCommission</Typography></Box><Button color="inherit" startIcon={<HomeRoundedIcon/>} onClick={()=>router.push("/app")}>Home</Button></Toolbar></AppBar>}
