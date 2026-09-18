"use client";

import { createTheme } from "@mui/material/styles";

export const theme = createTheme({
  palette: {
    mode: "light",
    primary: { main: "#0F7056" },
    secondary: { main: "#10B981" },
    success: { main: "#10B981", light: "#F0FDF4" },
    info: { main: "#2563EB", light: "#EFF6FF" },
    warning: { main: "#F59E0B", light: "#FFF7E7" },
    error: { main: "#DC2626", light: "#FEECEC" },
    text: {
      primary: "#1F2937",
      secondary: "#667280",
    },
    divider: "#BFC7D0",
    background: {
      default: "#F5F7F8",
      paper: "#FFFFFF",
    },
  },
  typography: {
    fontFamily: "Inter, Arial, sans-serif",
    h1: { fontSize: "2rem", fontWeight: 700 },
    h2: { fontSize: "1.5rem", fontWeight: 700 },
    h3: { fontSize: "1.25rem", fontWeight: 600 },
    body1: { fontSize: "1rem", lineHeight: 1.5 },
    body2: { fontSize: "0.875rem", lineHeight: 1.5 },
  },
  shape: {
    borderRadius: 10,
  },
  spacing: 4,
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          minHeight: 40,
          borderRadius: 9,
          textTransform: "none",
          fontWeight: 600,
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 14,
          boxShadow: "none",
        },
      },
    },
  },
});
