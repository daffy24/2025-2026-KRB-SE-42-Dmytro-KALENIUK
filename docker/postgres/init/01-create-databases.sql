SELECT 'CREATE DATABASE education'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'education')\gexec

SELECT 'CREATE DATABASE keycloak'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'keycloak')\gexec
