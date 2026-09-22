CREATE TEMP TABLE tmp_default_clinic AS
SELECT t.id AS tenant_id, gen_random_uuid() AS clinic_id
FROM core.tenant t
WHERE NOT EXISTS (SELECT 1 FROM business.clinic c WHERE c.tenant_id = t.id);

INSERT INTO business.clinic (id, tenant_id, name, active)
SELECT clinic_id, tenant_id, 'Clínica principal', true
FROM tmp_default_clinic;

ALTER TABLE business.professional ADD COLUMN clinic_id uuid;
ALTER TABLE business.professional_role ADD COLUMN clinic_id uuid;
ALTER TABLE business.professional_specialty ADD COLUMN clinic_id uuid;

UPDATE business.professional p SET clinic_id = c.id
FROM business.clinic c WHERE c.tenant_id = p.tenant_id AND c.id = (SELECT c2.id FROM business.clinic c2 WHERE c2.tenant_id = p.tenant_id ORDER BY c2.id LIMIT 1);
UPDATE business.professional_role p SET clinic_id = c.id
FROM business.clinic c WHERE c.tenant_id = p.tenant_id AND c.id = (SELECT c2.id FROM business.clinic c2 WHERE c2.tenant_id = p.tenant_id ORDER BY c2.id LIMIT 1);
UPDATE business.professional_specialty p SET clinic_id = c.id
FROM business.clinic c WHERE c.tenant_id = p.tenant_id AND c.id = (SELECT c2.id FROM business.clinic c2 WHERE c2.tenant_id = p.tenant_id ORDER BY c2.id LIMIT 1);

ALTER TABLE business.professional ALTER COLUMN clinic_id SET NOT NULL;
ALTER TABLE business.professional_role ALTER COLUMN clinic_id SET NOT NULL;
ALTER TABLE business.professional_specialty ALTER COLUMN clinic_id SET NOT NULL;

ALTER TABLE business.professional ADD CONSTRAINT fk_professional_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT;
ALTER TABLE business.professional_role ADD CONSTRAINT fk_professional_role_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT;
ALTER TABLE business.professional_specialty ADD CONSTRAINT fk_professional_specialty_clinic FOREIGN KEY (clinic_id) REFERENCES business.clinic (id) ON DELETE RESTRICT;

CREATE INDEX ix_professional_tenant_clinic_active ON business.professional (tenant_id, clinic_id, active);
CREATE INDEX ix_professional_role_tenant_clinic_active ON business.professional_role (tenant_id, clinic_id, active);
CREATE INDEX ix_professional_specialty_tenant_clinic_active ON business.professional_specialty (tenant_id, clinic_id, active);
