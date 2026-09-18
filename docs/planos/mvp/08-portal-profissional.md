# Portal do profissional — plano da etapa

**Objetivo:** oferecer extrato pessoal mobile-first sem permitir seleção ou acesso a outro profissional.

**Branch:** `<linear-id>-portal-profissional`  
**Dependência:** etapa 06; pode evoluir em paralelo à etapa 07 depois dos contratos estabilizados.

## Escopo

- Dashboard pessoal, meus procedimentos, detalhe, minhas comissões e extrato.
- Rotas sob `app/(professional)/portal` e navegação inferior.
- Endpoints `/api/me/...` derivados da identidade autenticada.
- Primeiro acesso simples conforme decisão D07.

## Tarefas pequenas

1. Escrever testes provando que IDs manipulados não revelam outro profissional.
2. Implementar queries pessoais e agregações do período.
3. Expor contratos enxutos para dashboard, lista, detalhe e extrato.
4. Implementar layout/header/bottom navigation mobile-first.
5. Implementar KPIs, listas e memória de cálculo.
6. Cobrir loading, vazio, erro, período fechado e acessibilidade.
7. Validar larguras mobile e desktop previstas nos requisitos.

## Conclusão

Profissional encontra comissão do período rapidamente, entende cada cálculo e nunca recebe dados de outro profissional.
