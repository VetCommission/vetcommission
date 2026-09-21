"use client";

import { Button, Stack, Typography } from "@mui/material";

export default function GlobalError({ reset }: { error: Error & { digest?: string }; reset: () => void }) {
  return <html lang="pt-BR"><body><Stack sx={{ alignItems: "center", gap: 2, p: 4 }}><Typography component="h1" variant="h5">Ocorreu um erro inesperado.</Typography><Button variant="contained" onClick={() => reset()}>Recarregar</Button></Stack></body></html>;
}
