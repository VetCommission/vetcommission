# Segurança multi-tenant

## Regra de autorização

O header `X-Tenant-Id` seleciona contexto, mas nunca constitui autoridade. Em cada endpoint tenant-scoped, a API valida conjuntamente:

1. a identidade autenticada (`userId`);
2. o tenant ativo informado no header;
3. o vínculo ativo do usuário com esse tenant;
4. o recurso exigido pela policy dentro do grupo de acesso desse mesmo tenant.

As permissões não são agregadas entre tenants no JWT. O token identifica o usuário; a autorização corrente é consultada no banco, permitindo que inativação de usuário, tenant, vínculo, grupo ou recurso tenha efeito sem aguardar um novo token.

Os repositórios de negócio continuam filtrando toda leitura e escrita por `TenantId` como segunda camada de proteção. Identificadores recebidos por rota ou payload não alteram o tenant resolvido pelo contexto autorizado.

## Endpoints sem tenant ativo

`POST /api/auth/login` é anônimo. `GET /api/auth/me` exige somente usuário autenticado porque restaura a sessão e devolve os tenants permitidos antes da escolha do tenant ativo.

Todos os endpoints atuais de profissionais, dados mestres, funções/cargos e especialidades são tenant-scoped e exigem a policy de recurso correspondente. Header ausente, inválido, tenant sem vínculo ou recurso ausente resulta em `403` sem consultar ou revelar dados do domínio.

## Vínculos de entidades

Quando um profissional é associado a um `userId`, a camada de aplicação confirma que esse usuário possui vínculo ativo com o tenant corrente. Um usuário pertencente somente a outro tenant não pode ser associado, mesmo que seu identificador seja conhecido.

## Cobertura obrigatória

Os testes de integração devem manter pelo menos dois tenants e validar acesso permitido, header adulterado, recurso pertencente a outro tenant, ausência de header, IDs de outro tenant e associação indevida de usuário. As respostas negativas não devem revelar a existência de registros de outro tenant.
