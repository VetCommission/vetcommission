import { Button, Stack, Typography } from "@mui/material";

export default function NotFound() {
  return (
    <Stack sx={{ alignItems: "center", gap: 2, p: 4 }}>
      <Typography component="h1" variant="h4">
        Página não encontrada
      </Typography>
      <Button href="/app" variant="contained">
        Voltar ao início
      </Button>
    </Stack>
  );
}
