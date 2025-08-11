-- Migración: Agregar campos Estado, NumeroFactura y Observaciones a tabla Compras
-- Fecha: 11 de agosto de 2025

-- Agregar columna Estado con valor por defecto
ALTER TABLE Compras 
ADD Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente';

-- Agregar columna NumeroFactura (opcional)
ALTER TABLE Compras 
ADD NumeroFactura NVARCHAR(50) NULL;

-- Agregar columna Observaciones (opcional)
ALTER TABLE Compras 
ADD Observaciones NVARCHAR(1000) NULL;

-- Actualizar registros existentes para que tengan estado 'Completada'
UPDATE Compras 
SET Estado = 'Completada' 
WHERE Estado = 'Pendiente';

PRINT 'Migración completada: Columnas Estado, NumeroFactura y Observaciones agregadas a tabla Compras';
