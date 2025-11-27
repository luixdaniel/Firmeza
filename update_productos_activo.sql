-- Script para actualizar todos los productos existentes a Activo = true
UPDATE "Productos" SET "Activo" = true WHERE "Activo" = false;

