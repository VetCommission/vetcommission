CREATE TABLE business.clinic
(
    id              uuid         NOT NULL,
    tenant_id       uuid         NOT NULL,
    name            varchar(160) NOT NULL,
    legal_name      varchar(200) NULL,
    document        varchar(30)  NULL,
    email           varchar(254) NULL,
    phone           varchar(40)  NULL,
    active          boolean      NOT NULL DEFAULT true,
    created_at_utc  timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc  timestamptz  NULL,
    inactivated_at_utc timestamptz NULL,

    CONSTRAINT pk_clinic PRIMARY KEY (id),
    CONSTRAINT fk_clinic_tenant FOREIGN KEY (tenant_id)
        REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT ck_clinic_name_not_blank CHECK (length(trim(name)) > 0),
    CONSTRAINT ck_clinic_active_dates CHECK (active OR inactivated_at_utc IS NOT NULL)
);

CREATE INDEX ix_clinic_tenant_active
    ON business.clinic (tenant_id, active);

CREATE INDEX ix_clinic_tenant_name
    ON business.clinic (tenant_id, name);

CREATE UNIQUE INDEX ux_clinic_tenant_document
    ON business.clinic (tenant_id, document)
    WHERE document IS NOT NULL;

INSERT INTO core.access_resource
(
    id, resource_key, name, description, active
)
VALUES
(
    '44444444-4444-4444-4444-444444444450',
    'menu.clinicas',
    'Menu de clínicas',
    'Permite visualizar o menu de clínicas.',
    true
),
(
    '44444444-4444-4444-4444-444444444451',
    'clinicas.gerenciar',
    'Gerenciar clínicas',
    'Permite consultar e alterar clínicas do tenant.',
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
    WHERE resource_key IN ('menu.clinicas', 'clinicas.gerenciar')
) AS resource_data
ON CONFLICT DO NOTHING;
