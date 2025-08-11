-- Script para corregir problemas de la base de datos Insumos
USE AetherEyeDB2;

-- 1. Primero verificar si existe la columna FechaUltimaActualizacion
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Insumos' AND COLUMN_NAME = 'FechaUltimaActualizacion')
BEGIN
    -- Renombrar la columna para que coincida con el modelo
    EXEC sp_rename 'Insumos.FechaUltimaActualizacion', 'FechaActualizacion', 'COLUMN';
    PRINT 'Columna FechaUltimaActualizacion renombrada a FechaActualizacion';
END

-- 2. Verificar y corregir valores NULL en campos requeridos
-- Actualizar registros con valores NULL en campos de string
UPDATE Insumos 
SET Nombre = 'Sin Nombre'
WHERE Nombre IS NULL;

UPDATE Insumos 
SET Categoria = 'General'
WHERE Categoria IS NULL;

UPDATE Insumos 
SET UnidadMedida = 'Unidad'
WHERE UnidadMedida IS NULL;

-- 3. Verificar que las columnas de fecha tengan valores por defecto
UPDATE Insumos 
SET FechaCreacion = GETDATE()
WHERE FechaCreacion IS NULL;

UPDATE Insumos 
SET FechaActualizacion = GETDATE()
WHERE FechaActualizacion IS NULL;

PRINT 'Correcciones aplicadas exitosamente';
