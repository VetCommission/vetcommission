CREATE TABLE business.procedure
(
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    clinic_id uuid NOT NULL,
    category_id uuid NOT NULL,
    code varchar(60) NULL,
    name varchar(160) NOT NULL,
    description varchar(500) NULL,
    default_value numeric(12,2) NOT NULL,
    active boolean NOT NULL DEFAULT true,
    created_at_utc timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc timestamptz NULL,
    inactivated_at_utc timestamptz NULL,
    CONSTRAINT pk_procedure PRIMARY KEY (id),
    CONSTRAINT fk_procedure_tenant FOREIGN KEY (tenant_id) REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT fk_procedure_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT,
    CONSTRAINT fk_procedure_category FOREIGN KEY (category_id) REFERENCES business.procedure_category (id) ON DELETE RESTRICT,
    CONSTRAINT ck_procedure_name_not_blank CHECK (length(trim(name)) > 0),
    CONSTRAINT ck_procedure_default_value_nonnegative CHECK (default_value >= 0),
    CONSTRAINT ck_procedure_active_dates CHECK (active OR inactivated_at_utc IS NOT NULL)
);

CREATE UNIQUE INDEX ux_procedure_tenant_clinic_code ON business.procedure (tenant_id, clinic_id, lower(code)) WHERE code IS NOT NULL;
CREATE UNIQUE INDEX ux_procedure_tenant_clinic_name ON business.procedure (tenant_id, clinic_id, lower(name));
CREATE INDEX ix_procedure_tenant_clinic_category_active ON business.procedure (tenant_id, clinic_id, category_id, active);

INSERT INTO core.access_resource (id, resource_key, name, description, active)
VALUES ('44444444-4444-4444-4444-444444444461', 'procedimentos.gerenciar', 'Gerenciar procedimentos', 'Permite consultar e alterar procedimentos da clinica.', true)
ON CONFLICT (resource_key) DO UPDATE SET name=EXCLUDED.name, description=EXCLUDED.description, active=EXCLUDED.active;

INSERT INTO core.access_group_resource (access_group_id, access_resource_id)
SELECT g.id, r.id FROM (VALUES ('33333333-3333-3333-3333-333333333331'::uuid), ('33333333-3333-3333-3333-333333333332'::uuid)) g(id)
JOIN core.access_resource r ON r.resource_key='procedimentos.gerenciar'
ON CONFLICT DO NOTHING;
