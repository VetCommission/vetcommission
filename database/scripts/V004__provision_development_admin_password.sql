-- Credencial exclusiva do ambiente local de desenvolvimento.
-- Nao reutilizar este hash em homologacao ou producao.
UPDATE core."user"
SET password_hash = 'v1.210000.ErmMwg4BkmJfgoQh4+8/qA==.Jfp6I3Nt/ndmhHmX8k+o+04/vGilpJOeBqvZ4suznbM=',
    password_updated_at_utc = CURRENT_TIMESTAMP,
    updated_at_utc = CURRENT_TIMESTAMP
WHERE normalized_email = 'ADMIN@VETCOMMISSION.LOCAL'
  AND active = true;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM core."user"
        WHERE normalized_email = 'ADMIN@VETCOMMISSION.LOCAL'
          AND password_hash = 'v1.210000.ErmMwg4BkmJfgoQh4+8/qA==.Jfp6I3Nt/ndmhHmX8k+o+04/vGilpJOeBqvZ4suznbM='
    ) THEN
        RAISE EXCEPTION 'Administrador inicial nao encontrado ou inativo para provisionamento local';
    END IF;
END $$;
