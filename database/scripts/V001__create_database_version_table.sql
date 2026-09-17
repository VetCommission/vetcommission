CREATE SCHEMA core;

CREATE SCHEMA business;

CREATE TABLE core.database_version
(
    id              bigint GENERATED ALWAYS AS IDENTITY,
    version         varchar(20)  NOT NULL,
    description     varchar(200) NOT NULL,
    script_name     varchar(255) NOT NULL,
    checksum_sha256 char(64)     NOT NULL,
    applied_at_utc  timestamptz  NOT NULL DEFAULT CURRENT_TIMESTAMP,
    applied_by      varchar(100) NOT NULL DEFAULT CURRENT_USER,

    CONSTRAINT pk_database_version
        PRIMARY KEY (id),

    CONSTRAINT uk_database_version_version
        UNIQUE (version),

    CONSTRAINT uk_database_version_script_name
        UNIQUE (script_name)
);
