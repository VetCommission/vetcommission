# ADR-015 — Clínica como contexto operacional

## Status

Aprovada para execução conforme decisão do produto.

## Decisão

`Tenant` representa a empresa ou grupo de clínicas. Uma clínica pertence a um tenant e representa o contexto operacional onde são mantidos profissionais, cadastros mestres, categorias, procedimentos, produção, comissões e fechamentos.

O tenant permanece como contexto de identidade, vínculo e autorização. A clínica é um contexto funcional separado.

## Regras

- todo dado operacional de clínica possui `tenant_id` e `clinic_id`;
- toda entidade pertencente a um tenant possui `tenant_id`, mesmo quando também possui `clinic_id`;
- índices e chaves estrangeiras compostas devem preservar a combinação `tenant_id` + contexto operacional;
- tenant com uma única clínica seleciona essa clínica automaticamente;
- tenant sem clínica não pode iniciar operações dependentes de clínica;
- trocar a clínica invalida o cache dos domínios operacionais;
- IDs de clínica de outro tenant nunca podem ser aceitos;
- unidades de uma clínica ficam fora do MVP.

## Impacto

A etapa 03.5 deve ser implementada antes do catálogo de categorias e procedimentos. Profissionais, funções, especialidades e demais domínios operacionais deverão receber migração corretiva para `clinic_id` antes de serem considerados prontos para múltiplas clínicas.
