# MATRIZ DE PERFIS, TELAS E AÇÕES

Legenda:
- V = visualizar
- C = criar
- E = editar
- D = decidir/aprovar/rejeitar
- F = fechar
- — = não disponível

| Área | Administrador | Operacional | Profissional |
|---|---|---|---|
| Dashboard administrativo | V | V | — |
| Profissionais | V/C/E | V | — |
| Procedimentos | V/C/E | V | — |
| Regras de comissão | V/C/E | V conforme permissão | — |
| Produção | V/C/E | V/C/E | somente próprios V |
| Comissões | V | V conforme permissão | somente próprias V |
| Contestações | V/D | V conforme permissão | próprias V/C |
| Fechamentos | V/F | V | próprios V |
| Relatórios administrativos | V | V conforme permissão | — |
| Dashboard profissional | — | — | V |
| Perfil próprio | V | V | V |

## Regras visuais derivadas

- Não exibir ação que o perfil nunca poderá executar.
- Se uma ação existir, mas estiver temporariamente bloqueada por estado, pode aparecer desabilitada com explicação.
- O profissional nunca deve visualizar seletor que permita acessar dados financeiros de outro profissional.
