import CheckCircleRoundedIcon from "@mui/icons-material/CheckCircleRounded";
import { Box, Chip, Container, Paper, Stack, Typography } from "@mui/material";

export default function Home() {
  return (
    <Container component="main" maxWidth="sm" sx={{ py: { xs: 6, md: 10 } }}>
      <Paper variant="outlined" sx={{ p: { xs: 3, md: 5 } }}>
        <Stack spacing={3}>
          <Box>
            <Typography component="p" color="primary" gutterBottom sx={{ fontWeight: 700 }}>
              VetCommission
            </Typography>
            <Typography component="h1" variant="h3">
              Fundação da aplicação pronta
            </Typography>
          </Box>

          <Typography color="text.secondary">
            Estrutura técnica inicial para a API, o Worker e o portal web. Os fluxos de
            negócio serão adicionados nas próximas etapas do MVP.
          </Typography>

          <Chip
            color="success"
            icon={<CheckCircleRoundedIcon />}
            label="Ambiente base configurado"
            sx={{ alignSelf: "flex-start" }}
          />
        </Stack>
      </Paper>
    </Container>
  );
}
