CREATE TABLE business.procedure_category
(
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    clinic_id uuid NOT NULL,
    name varchar(160) NOT NULL,
    description varchar(500) NULL,
    active boolean NOT NULL DEFAULT true,
    created_at_utc timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc timestamptz NULL,
    inactivated_at_utc timestamptz NULL,
    CONSTRAINT pk_procedure_category PRIMARY KEY (id),
    CONSTRAINT fk_procedure_category_tenant FOREIGN KEY (tenant_id) REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT fk_procedure_category_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT,
    CONSTRAINT ck_procedure_category_name_not_blank CHECK (length(trim(name)) > 0),
    CONSTRAINT ck_procedure_category_active_dates CHECK (active OR inactivated_at_utc IS NOT NULL)
);

CREATE UNIQUE INDEX ux_procedure_category_tenant_clinic_name ON business.procedure_category (tenant_id, clinic_id, lower(name));
CREATE INDEX ix_procedure_category_tenant_clinic_active ON business.procedure_category (tenant_id, clinic_id, active);

INSERT INTO core.access_resource (id, resource_key, name, description, active)
VALUES ('44444444-4444-4444-4444-444444444460', 'categorias-procedimentos.gerenciar', 'Gerenciar categorias', 'Permite consultar e alterar categorias de procedimentos.', true)
ON CONFLICT (resource_key) DO UPDATE SET name=EXCLUDED.name, description=EXCLUDED.description, active=EXCLUDED.active;

INSERT INTO core.access_group_resource (access_group_id, access_resource_id)
SELECT g.id, r.id FROM (VALUES ('33333333-3333-3333-3333-333333333331'::uuid), ('33333333-3333-3333-3333-333333333332'::uuid)) g(id)
JOIN core.access_resource r ON r.resource_key='categorias-procedimentos.gerenciar'
ON CONFLICT DO NOTHING;
