# Processo de etapa, branch, PR e Linear

## Unidade de entrega

Cada etapa `00`–`12` corresponde a:

- uma issue principal no Linear;
- uma branch criada a partir da `main` atualizada;
- um conjunto de commits coesos;
- um Pull Request para `main`;
- um gate de revisão e testes;
- merge e encerramento da issue.

As tarefas pequenas do arquivo de plano ficam como checklist na descrição da issue ou no PR. Criar sub-issues somente quando partes puderem ser atribuídas, revisadas e entregues independentemente sem quebrar a branch única da etapa.

## Fluxo

```text
Backlog
→ Ready
→ In Progress
→ branch
→ commits
→ Pull Request
→ In Review / Testing
→ merge
→ Done
```

Os nomes exatos dos status devem ser mapeados ao workflow do time no momento da importação.

## Branch

Depois que o Linear gerar o identificador:

```text
<issue-id-em-minusculo>-<slug-curto>
```

Exemplos:

```text
vet-101-fundacao-solucao
vet-102-identidade-tenant-acesso
vet-103-profissionais
```

Uma branch parte da `main` atualizada. Não reutilizar branch concluída e não misturar duas etapas.

## Commits

Formato recomendado:

```text
VET-101 feat: cria estrutura inicial da solução
VET-101 test: cobre pipeline de validação
VET-101 docs: registra verificações da fundação
```

Commits intermediários devem permanecer compiláveis sempre que razoável. Segredos, arquivos `.env` reais e artefatos locais não entram no commit.

## Pull Request

Título:

```text
VET-101 — Fundação da solução
```

Descrição mínima:

```markdown
## Objetivo
## Escopo entregue
## Fora do escopo
## Banco e scripts
## Evidências de testes
## Evidências de UI/UX
## Riscos e pendências
## Checklist de conclusão
```

O PR deve apontar para a issue e para o plano da etapa. Alteração persistente lista scripts aplicados e resultado da segunda execução. Alteração visual inclui larguras verificadas.

## Merge e encerramento

Somente realizar merge quando:

- diff revisado;
- CI/build/testes aplicáveis aprovados;
- critérios do plano atendidos;
- pendências não bloqueantes registradas;
- documentação atualizada.

Após merge, remover a branch e marcar a issue como Done. A próxima etapa começa de uma nova branch criada da `main` atualizada.
