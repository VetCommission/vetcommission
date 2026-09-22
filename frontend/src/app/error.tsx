"use client";

import { Alert, Button, Stack } from "@mui/material";

export default function Error({
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  return (
    <Stack sx={{ alignItems: "center", gap: 2, p: 4 }}>
      <Alert severity="error">Não foi possível carregar esta página.</Alert>
      <Button variant="contained" onClick={() => reset()}>
        Tentar novamente
      </Button>
    </Stack>
  );
}
