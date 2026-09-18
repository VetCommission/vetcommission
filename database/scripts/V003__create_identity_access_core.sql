CREATE TABLE core.tenant
(
    id             uuid         NOT NULL,
    name           varchar(160) NOT NULL,
    slug           varchar(80)  NOT NULL,
    timezone       varchar(80)  NOT NULL DEFAULT 'America/Sao_Paulo',
    active         boolean      NOT NULL DEFAULT true,
    created_at_utc timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc timestamptz  NULL,

    CONSTRAINT pk_tenant
        PRIMARY KEY (id),

    CONSTRAINT uk_tenant_slug
        UNIQUE (slug),

    CONSTRAINT ck_tenant_name_not_blank
        CHECK (length(trim(name)) > 0),

    CONSTRAINT ck_tenant_slug_not_blank
        CHECK (length(trim(slug)) > 0),

    CONSTRAINT ck_tenant_timezone_not_blank
        CHECK (length(trim(timezone)) > 0)
);

CREATE TABLE core."user"
(
    id                      uuid         NOT NULL,
    name                    varchar(160) NOT NULL,
    email                   varchar(254) NOT NULL,
    normalized_email        varchar(254) NOT NULL,
    password_hash           text         NOT NULL,
    active                  boolean      NOT NULL DEFAULT true,
    created_at_utc          timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc          timestamptz  NULL,
    password_updated_at_utc timestamptz  NULL,
    last_login_at_utc       timestamptz  NULL,

    CONSTRAINT pk_user
        PRIMARY KEY (id),

    CONSTRAINT uk_user_normalized_email
        UNIQUE (normalized_email),

    CONSTRAINT ck_user_name_not_blank
        CHECK (length(trim(name)) > 0),

    CONSTRAINT ck_user_email_not_blank
        CHECK (length(trim(email)) > 0),

    CONSTRAINT ck_user_normalized_email_not_blank
        CHECK (length(trim(normalized_email)) > 0),

    CONSTRAINT ck_user_password_hash_not_blank
        CHECK (length(trim(password_hash)) > 0)
);

CREATE TABLE core.access_group
(
    id             uuid         NOT NULL,
    tenant_id      uuid         NOT NULL,
    code           varchar(80)  NOT NULL,
    name           varchar(120) NOT NULL,
    description    varchar(300) NULL,
    active         boolean      NOT NULL DEFAULT true,
    created_at_utc timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc timestamptz  NULL,

    CONSTRAINT pk_access_group
        PRIMARY KEY (id),

    CONSTRAINT fk_access_group_tenant
        FOREIGN KEY (tenant_id)
        REFERENCES core.tenant (id)
        ON DELETE RESTRICT,

    CONSTRAINT uk_access_group_tenant_id_code
        UNIQUE (tenant_id, code),

    CONSTRAINT uk_access_group_id_tenant_id
        UNIQUE (id, tenant_id),

    CONSTRAINT ck_access_group_code_not_blank
        CHECK (length(trim(code)) > 0),

    CONSTRAINT ck_access_group_name_not_blank
        CHECK (length(trim(name)) > 0)
);

CREATE TABLE core.access_resource
(
    id             uuid         NOT NULL,
    resource_key   varchar(160) NOT NULL,
    name           varchar(160) NOT NULL,
    description    varchar(300) NULL,
    active         boolean      NOT NULL DEFAULT true,
    created_at_utc timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc timestamptz  NULL,

    CONSTRAINT pk_access_resource
        PRIMARY KEY (id),

    CONSTRAINT uk_access_resource_resource_key
        UNIQUE (resource_key),

    CONSTRAINT ck_access_resource_key_not_blank
        CHECK (length(trim(resource_key)) > 0),

    CONSTRAINT ck_access_resource_name_not_blank
        CHECK (length(trim(name)) > 0)
);

CREATE TABLE core.access_group_resource
(
    access_group_id    uuid        NOT NULL,
    access_resource_id uuid        NOT NULL,
    created_at_utc     timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_access_group_resource
        PRIMARY KEY (access_group_id, access_resource_id),

    CONSTRAINT fk_access_group_resource_access_group
        FOREIGN KEY (access_group_id)
        REFERENCES core.access_group (id)
        ON DELETE CASCADE,

    CONSTRAINT fk_access_group_resource_access_resource
        FOREIGN KEY (access_resource_id)
        REFERENCES core.access_resource (id)
        ON DELETE RESTRICT
);

CREATE TABLE core.user_tenant
(
    id              uuid        NOT NULL,
    user_id         uuid        NOT NULL,
    tenant_id       uuid        NOT NULL,
    access_group_id uuid        NOT NULL,
    active          boolean     NOT NULL DEFAULT true,
    is_default      boolean     NOT NULL DEFAULT false,
    created_at_utc  timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at_utc  timestamptz NULL,

    CONSTRAINT pk_user_tenant
        PRIMARY KEY (id),

    CONSTRAINT fk_user_tenant_user
        FOREIGN KEY (user_id)
        REFERENCES core."user" (id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_user_tenant_tenant
        FOREIGN KEY (tenant_id)
        REFERENCES core.tenant (id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_user_tenant_access_group_same_tenant
        FOREIGN KEY (access_group_id, tenant_id)
        REFERENCES core.access_group (id, tenant_id)
        ON DELETE RESTRICT,

    CONSTRAINT uk_user_tenant_user_id_tenant_id
        UNIQUE (user_id, tenant_id)
);

CREATE UNIQUE INDEX ux_user_tenant_default
    ON core.user_tenant (user_id, (CASE WHEN is_default THEN true ELSE NULL END))
    WHERE is_default;

CREATE INDEX ix_access_group_tenant_id_active
    ON core.access_group (tenant_id, active);

CREATE INDEX ix_access_group_resource_access_resource_id
    ON core.access_group_resource (access_resource_id);

CREATE INDEX ix_user_tenant_tenant_id_active
    ON core.user_tenant (tenant_id, active);

CREATE INDEX ix_user_tenant_user_id_active
    ON core.user_tenant (user_id, active);

INSERT INTO core.tenant
(
    id,
    name,
    slug,
    timezone,
    active
)
VALUES
(
    '11111111-1111-1111-1111-111111111111',
    'Tenant Inicial',
    'tenant-inicial',
    'America/Sao_Paulo',
    true
);

INSERT INTO core."user"
(
    id,
    name,
    email,
    normalized_email,
    password_hash,
    active
)
VALUES
(
    '22222222-2222-2222-2222-222222222222',
    'Administrador Inicial',
    'admin@vetcommission.local',
    'ADMIN@VETCOMMISSION.LOCAL',
    'OPERATIONAL_PASSWORD_HASH_REQUIRED',
    true
);

INSERT INTO core.access_group
(
    id,
    tenant_id,
    code,
    name,
    description,
    active
)
VALUES
(
    '33333333-3333-3333-3333-333333333331',
    '11111111-1111-1111-1111-111111111111',
    'ADMINISTRADOR',
    'Administrador',
    'Acesso administrativo completo ao tenant.',
    true
),
(
    '33333333-3333-3333-3333-333333333332',
    '11111111-1111-1111-1111-111111111111',
    'OPERACIONAL',
    'Operacional',
    'Acesso operacional administrativo ao tenant.',
    true
),
(
    '33333333-3333-3333-3333-333333333333',
    '11111111-1111-1111-1111-111111111111',
    'PROFISSIONAL',
    'Profissional',
    'Acesso ao portal profissional.',
    true
);

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
    '44444444-4444-4444-4444-444444444441',
    'app.access',
    'Acessar aplicação',
    'Permite acessar a área autenticada da aplicação.',
    true
),
(
    '44444444-4444-4444-4444-444444444442',
    'admin.dashboard',
    'Dashboard administrativo',
    'Permite acessar a área administrativa inicial.',
    true
),
(
    '44444444-4444-4444-4444-444444444443',
    'professional.portal',
    'Portal profissional',
    'Permite acessar a área do profissional.',
    true
);

INSERT INTO core.access_group_resource
(
    access_group_id,
    access_resource_id
)
VALUES
(
    '33333333-3333-3333-3333-333333333331',
    '44444444-4444-4444-4444-444444444441'
),
(
    '33333333-3333-3333-3333-333333333331',
    '44444444-4444-4444-4444-444444444442'
),
(
    '33333333-3333-3333-3333-333333333331',
    '44444444-4444-4444-4444-444444444443'
),
(
    '33333333-3333-3333-3333-333333333332',
    '44444444-4444-4444-4444-444444444441'
),
(
    '33333333-3333-3333-3333-333333333332',
    '44444444-4444-4444-4444-444444444442'
),
(
    '33333333-3333-3333-3333-333333333333',
    '44444444-4444-4444-4444-444444444441'
),
(
    '33333333-3333-3333-3333-333333333333',
    '44444444-4444-4444-4444-444444444443'
);

INSERT INTO core.user_tenant
(
    id,
    user_id,
    tenant_id,
    access_group_id,
    active,
    is_default
)
VALUES
(
    '55555555-5555-5555-5555-555555555551',
    '22222222-2222-2222-2222-222222222222',
    '11111111-1111-1111-1111-111111111111',
    '33333333-3333-3333-3333-333333333331',
    true,
    true
);
