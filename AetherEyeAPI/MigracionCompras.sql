/*
MIGRACIÓN MANUAL PARA TABLA COMPRAS
Ejecutar estos comandos SQL directamente en la base de datos
*/

-- Verificar si las columnas ya existen antes de agregarlas

-- 1. Agregar columna Estado
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Compras' AND COLUMN_NAME = 'Estado')
BEGIN
    ALTER TABLE Compras ADD Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
    PRINT 'Columna Estado agregada exitosamente'
END
ELSE
BEGIN
    PRINT 'Columna Estado ya existe'
END

-- 2. Agregar columna NumeroFactura
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Compras' AND COLUMN_NAME = 'NumeroFactura')
BEGIN
    ALTER TABLE Compras ADD NumeroFactura NVARCHAR(50) NULL
    PRINT 'Columna NumeroFactura agregada exitosamente'
END
ELSE
BEGIN
    PRINT 'Columna NumeroFactura ya existe'
END

-- 3. Agregar columna Observaciones
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Compras' AND COLUMN_NAME = 'Observaciones')
BEGIN
    ALTER TABLE Compras ADD Observaciones NVARCHAR(1000) NULL
    PRINT 'Columna Observaciones agregada exitosamente'
END
ELSE
BEGIN
    PRINT 'Columna Observaciones ya existe'
END

-- Verificar que las columnas se agregaron correctamente
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Compras' 
AND COLUMN_NAME IN ('Estado', 'NumeroFactura', 'Observaciones')
ORDER BY COLUMN_NAME
