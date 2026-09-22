CREATE TABLE business.professional_role
(
    id              uuid         NOT NULL,
    tenant_id       uuid         NOT NULL,
    name            varchar(120) NOT NULL,
    active          boolean      NOT NULL DEFAULT true,
    created_at_utc  timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc  timestamptz  NULL,

    CONSTRAINT pk_professional_role PRIMARY KEY (id),
    CONSTRAINT fk_professional_role_tenant FOREIGN KEY (tenant_id)
        REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT ck_professional_role_name_not_blank CHECK (length(trim(name)) > 0)
);

CREATE UNIQUE INDEX ux_professional_role_tenant_name
    ON business.professional_role (tenant_id, lower(name));
CREATE INDEX ix_professional_role_tenant_active
    ON business.professional_role (tenant_id, active);

CREATE TABLE business.professional_specialty
(
    id              uuid         NOT NULL,
    tenant_id       uuid         NOT NULL,
    name            varchar(120) NOT NULL,
    active          boolean      NOT NULL DEFAULT true,
    created_at_utc  timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc  timestamptz  NULL,

    CONSTRAINT pk_professional_specialty PRIMARY KEY (id),
    CONSTRAINT fk_professional_specialty_tenant FOREIGN KEY (tenant_id)
        REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT ck_professional_specialty_name_not_blank CHECK (length(trim(name)) > 0)
);

CREATE UNIQUE INDEX ux_professional_specialty_tenant_name
    ON business.professional_specialty (tenant_id, lower(name));
CREATE INDEX ix_professional_specialty_tenant_active
    ON business.professional_specialty (tenant_id, active);

INSERT INTO core.access_resource (id, resource_key, name, description, active)
VALUES
    ('44444444-4444-4444-4444-444444444446', 'menu.funcoes-cargos', 'Menu de funções e cargos', 'Permite visualizar funções e cargos.', true),
    ('44444444-4444-4444-4444-444444444447', 'funcoes-cargos.gerenciar', 'Gerenciar funções e cargos', 'Permite consultar e alterar funções e cargos.', true),
    ('44444444-4444-4444-4444-444444444448', 'menu.especialidades', 'Menu de especialidades', 'Permite visualizar especialidades.', true),
    ('44444444-4444-4444-4444-444444444449', 'especialidades.gerenciar', 'Gerenciar especialidades', 'Permite consultar e alterar especialidades.', true)
ON CONFLICT (resource_key) DO UPDATE SET name = EXCLUDED.name, description = EXCLUDED.description, active = EXCLUDED.active;

INSERT INTO core.access_group_resource (access_group_id, access_resource_id)
SELECT group_data.id, resource_data.id
FROM (VALUES
    ('33333333-3333-3333-3333-333333333331'::uuid),
    ('33333333-3333-3333-3333-333333333332'::uuid)
) AS group_data(id)
CROSS JOIN (
    SELECT id FROM core.access_resource
    WHERE resource_key IN ('menu.funcoes-cargos', 'funcoes-cargos.gerenciar', 'menu.especialidades', 'especialidades.gerenciar')
) AS resource_data
ON CONFLICT DO NOTHING;
