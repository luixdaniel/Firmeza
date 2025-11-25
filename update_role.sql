-- Script para actualizar el rol Administrador a Admin en PostgreSQL
-- Ejecutar este script en la base de datos firmeza_db

-- Actualizar el nombre del rol
UPDATE "AspNetRoles" 
SET "Name" = 'Admin', "NormalizedName" = 'ADMIN' 
WHERE "Name" = 'Administrador';

-- Confirmar cambios
SELECT * FROM "AspNetRoles" WHERE "Name" IN ('Admin', 'Administrador');

