CREATE TABLE business.competency
(
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    clinic_id uuid NOT NULL,
    year smallint NOT NULL,
    month smallint NOT NULL,
    status varchar(20) NOT NULL DEFAULT 'open',
    opened_at_utc timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    closed_at_utc timestamptz NULL,
    CONSTRAINT pk_competency PRIMARY KEY (id),
    CONSTRAINT fk_competency_tenant FOREIGN KEY (tenant_id) REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT fk_competency_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT,
    CONSTRAINT ck_competency_month CHECK (month BETWEEN 1 AND 12),
    CONSTRAINT ck_competency_year CHECK (year BETWEEN 2000 AND 2200),
    CONSTRAINT ck_competency_status CHECK (status IN ('open','closed')),
    CONSTRAINT ck_competency_closed_date CHECK (status = 'open' OR closed_at_utc IS NOT NULL)
);
CREATE UNIQUE INDEX ux_competency_tenant_clinic_year_month ON business.competency (tenant_id, clinic_id, year, month);
CREATE INDEX ix_competency_tenant_clinic_status ON business.competency (tenant_id, clinic_id, status);

CREATE TABLE business.commission_rule
(
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    clinic_id uuid NOT NULL,
    competency_id uuid NOT NULL,
    procedure_id uuid NOT NULL,
    rule_type varchar(20) NOT NULL,
    percentage numeric(7,4) NULL,
    fixed_value numeric(18,2) NULL,
    active boolean NOT NULL DEFAULT true,
    created_at_utc timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc timestamptz NULL,
    CONSTRAINT pk_commission_rule PRIMARY KEY (id),
    CONSTRAINT fk_commission_rule_tenant FOREIGN KEY (tenant_id) REFERENCES core.tenant (id) ON DELETE RESTRICT,
    CONSTRAINT fk_commission_rule_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT,
    CONSTRAINT fk_commission_rule_competency FOREIGN KEY (competency_id) REFERENCES business.competency (id) ON DELETE RESTRICT,
    CONSTRAINT fk_commission_rule_procedure FOREIGN KEY (procedure_id) REFERENCES business.procedure (id) ON DELETE RESTRICT,
    CONSTRAINT ck_commission_rule_type CHECK (rule_type IN ('percentage','fixed')),
    CONSTRAINT ck_commission_rule_value CHECK ((rule_type = 'percentage' AND percentage > 0 AND percentage <= 100 AND fixed_value IS NULL) OR (rule_type = 'fixed' AND fixed_value > 0 AND percentage IS NULL))
);
CREATE UNIQUE INDEX ux_commission_rule_competency_procedure ON business.commission_rule (tenant_id, clinic_id, competency_id, procedure_id);
CREATE INDEX ix_commission_rule_tenant_clinic_procedure ON business.commission_rule (tenant_id, clinic_id, procedure_id, active);

INSERT INTO core.access_resource (id, resource_key, name, description, active)
VALUES
 ('44444444-4444-4444-4444-444444444470','competencias.gerenciar','Gerenciar competencias','Permite abrir e fechar competencias.',true),
 ('44444444-4444-4444-4444-444444444471','regras-comissao.gerenciar','Gerenciar regras de comissao','Permite configurar regras gerais de comissao.',true)
ON CONFLICT (resource_key) DO UPDATE SET name=EXCLUDED.name,description=EXCLUDED.description,active=EXCLUDED.active;
INSERT INTO core.access_group_resource (access_group_id,access_resource_id)
SELECT g.id,r.id FROM (VALUES ('33333333-3333-3333-3333-333333333331'::uuid),('33333333-3333-3333-3333-333333333332'::uuid)) g(id) CROSS JOIN core.access_resource r WHERE r.resource_key IN ('competencias.gerenciar','regras-comissao.gerenciar') ON CONFLICT DO NOTHING;
