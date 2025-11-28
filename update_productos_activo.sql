-- Actualizar todos los productos existentes para que estén activos
UPDATE "Productos" SET "Activo" = true WHERE "Activo" = false;

-- Verificar el resultado
SELECT "Id", "Nombre", "Activo", "Stock" FROM "Productos";

