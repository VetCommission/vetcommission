CREATE TABLE business.professional
(
    id                          uuid         NOT NULL,
    tenant_id                   uuid         NOT NULL,
    user_id                     uuid         NULL,
    name                        varchar(160) NOT NULL,
    email                       varchar(254) NULL,
    phone                       varchar(40)  NULL,
    role                        varchar(120) NOT NULL,
    professional_registration   varchar(80)  NULL,
    specialty                   varchar(120) NULL,
    active                      boolean      NOT NULL DEFAULT true,
    created_at_utc              timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc              timestamptz  NULL,
    inactivated_at_utc          timestamptz  NULL,

    CONSTRAINT pk_professional
        PRIMARY KEY (id),

    CONSTRAINT fk_professional_tenant
        FOREIGN KEY (tenant_id)
        REFERENCES core.tenant (id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_professional_user
        FOREIGN KEY (user_id)
        REFERENCES core."user" (id)
        ON DELETE RESTRICT,

    CONSTRAINT ck_professional_name_not_blank
        CHECK (length(trim(name)) > 0),

    CONSTRAINT ck_professional_role_not_blank
        CHECK (length(trim(role)) > 0),

    CONSTRAINT ck_professional_email_not_blank
        CHECK (email IS NULL OR length(trim(email)) > 0),

    CONSTRAINT ck_professional_active_dates
        CHECK (active OR inactivated_at_utc IS NOT NULL)
);

CREATE INDEX ix_professional_tenant_id_active
    ON business.professional (tenant_id, active);

CREATE INDEX ix_professional_tenant_id_name
    ON business.professional (tenant_id, name);

CREATE UNIQUE INDEX ux_professional_tenant_id_email
    ON business.professional (tenant_id, lower(email))
    WHERE email IS NOT NULL;

CREATE UNIQUE INDEX ux_professional_tenant_id_user_id
    ON business.professional (tenant_id, user_id)
    WHERE user_id IS NOT NULL;

INSERT INTO core.access_resource
(
    id,
    resource_key,
    name,
    description,
    active
)
VALUES
(
    '44444444-4444-4444-4444-444444444444',
    'menu.profissionais',
    'Menu de profissionais',
    'Permite visualizar o menu de profissionais.',
    true
),
(
    '44444444-4444-4444-4444-444444444445',
    'profissionais.gerenciar',
    'Gerenciar profissionais',
    'Permite consultar e alterar profissionais do tenant.',
    true
)
ON CONFLICT (resource_key) DO UPDATE
SET name = EXCLUDED.name,
    description = EXCLUDED.description,
    active = EXCLUDED.active;

INSERT INTO core.access_group_resource (access_group_id, access_resource_id)
SELECT group_data.id, resource_data.id
FROM (VALUES
    ('33333333-3333-3333-3333-333333333331'::uuid),
    ('33333333-3333-3333-3333-333333333332'::uuid)
) AS group_data(id)
CROSS JOIN (
    SELECT id
    FROM core.access_resource
    WHERE resource_key IN ('menu.profissionais', 'profissionais.gerenciar')
) AS resource_data
ON CONFLICT DO NOTHING;
