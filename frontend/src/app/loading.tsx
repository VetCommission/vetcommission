import { CircularProgress, Stack } from "@mui/material";

export default function Loading() {
  return (
    <Stack sx={{ alignItems: "center", justifyContent: "center", minHeight: "40vh" }}>
      <CircularProgress aria-label="Carregando" />
    </Stack>
  );
}
