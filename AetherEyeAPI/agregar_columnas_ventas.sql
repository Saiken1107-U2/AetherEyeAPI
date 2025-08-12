-- Script para agregar columnas faltantes a la tabla Ventas

-- Agregar columnas de información del cliente
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'NombreCliente')
BEGIN
    ALTER TABLE [Ventas] ADD [NombreCliente] nvarchar(100) NULL
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'CorreoCliente')
BEGIN
    ALTER TABLE [Ventas] ADD [CorreoCliente] nvarchar(100) NULL
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'TelefonoCliente')
BEGIN
    ALTER TABLE [Ventas] ADD [TelefonoCliente] nvarchar(20) NULL
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'DireccionCliente')
BEGIN
    ALTER TABLE [Ventas] ADD [DireccionCliente] nvarchar(200) NULL
END

-- Agregar columnas de información de la venta
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Estado')
BEGIN
    ALTER TABLE [Ventas] ADD [Estado] nvarchar(50) NOT NULL DEFAULT 'Pendiente'
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'MetodoPago')
BEGIN
    ALTER TABLE [Ventas] ADD [MetodoPago] nvarchar(50) NULL
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Observaciones')
BEGIN
    ALTER TABLE [Ventas] ADD [Observaciones] nvarchar(1000) NULL
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'NumeroFactura')
BEGIN
    ALTER TABLE [Ventas] ADD [NumeroFactura] nvarchar(50) NULL
END

-- Agregar columnas de cálculos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Subtotal')
BEGIN
    ALTER TABLE [Ventas] ADD [Subtotal] decimal(18,2) NOT NULL DEFAULT 0
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Impuestos')
BEGIN
    ALTER TABLE [Ventas] ADD [Impuestos] decimal(18,2) NOT NULL DEFAULT 0
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Ventas]') AND name = 'Descuento')
BEGIN
    ALTER TABLE [Ventas] ADD [Descuento] decimal(18,2) NOT NULL DEFAULT 0
END

-- Verificar que las columnas se agregaron correctamente
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length,
    c.is_nullable
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('Ventas')
ORDER BY c.column_id
