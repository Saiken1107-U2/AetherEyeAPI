-- Script para actualizar las credenciales de prueba con contraseñas más seguras
-- Ejecutar este script en SQL Server Management Studio o Azure Data Studio

USE [AetherEyeDB2]; -- Cambiar por el nombre de tu base de datos si es diferente
GO

-- Verificar si existen los roles
IF NOT EXISTS (SELECT 1 FROM Roles WHERE Nombre = 'Admin')
BEGIN
    INSERT INTO Roles (Nombre) VALUES ('Admin');
    PRINT 'Rol Admin creado';
END
ELSE
BEGIN
    PRINT 'Rol Admin ya existe';
END

IF NOT EXISTS (SELECT 1 FROM Roles WHERE Nombre = 'Cliente')
BEGIN
    INSERT INTO Roles (Nombre) VALUES ('Cliente');
    PRINT 'Rol Cliente creado';
END
ELSE
BEGIN
    PRINT 'Rol Cliente ya existe';
END

-- Obtener IDs de los roles
DECLARE @AdminRoleId INT = (SELECT Id FROM Roles WHERE Nombre = 'Admin');
DECLARE @ClienteRoleId INT = (SELECT Id FROM Roles WHERE Nombre = 'Cliente');

-- Actualizar o insertar usuario Admin
IF EXISTS (SELECT 1 FROM Usuarios WHERE Correo = 'admin@aethereye.com')
BEGIN
    UPDATE Usuarios 
    SET Contrasena = 'AetherAdmin2025!',
        NombreCompleto = 'Administrador',
        RolId = @AdminRoleId,
        EstaActivo = 1,
        FechaRegistro = GETDATE()
    WHERE Correo = 'admin@aethereye.com';
    PRINT 'Usuario Admin actualizado con nueva contraseña';
END
ELSE
BEGIN
    INSERT INTO Usuarios (NombreCompleto, Correo, Contrasena, RolId, FechaRegistro, EstaActivo)
    VALUES ('Administrador', 'admin@aethereye.com', 'AetherAdmin2025!', @AdminRoleId, GETDATE(), 1);
    PRINT 'Usuario Admin creado';
END

-- Actualizar o insertar usuario Cliente
IF EXISTS (SELECT 1 FROM Usuarios WHERE Correo = 'cliente@aethereye.com')
BEGIN
    UPDATE Usuarios 
    SET Contrasena = 'AetherCliente2025!',
        NombreCompleto = 'Cliente Prueba',
        RolId = @ClienteRoleId,
        EstaActivo = 1,
        FechaRegistro = GETDATE()
    WHERE Correo = 'cliente@aethereye.com';
    PRINT 'Usuario Cliente actualizado con nueva contraseña';
END
ELSE
BEGIN
    INSERT INTO Usuarios (NombreCompleto, Correo, Contrasena, RolId, FechaRegistro, EstaActivo)
    VALUES ('Cliente Prueba', 'cliente@aethereye.com', 'AetherCliente2025!', @ClienteRoleId, GETDATE(), 1);
    PRINT 'Usuario Cliente creado';
END

-- Verificar que los usuarios fueron creados/actualizados correctamente
SELECT 
    u.Id,
    u.NombreCompleto,
    u.Correo,
    u.Contrasena,
    r.Nombre AS Rol,
    u.FechaRegistro,
    u.EstaActivo
FROM Usuarios u
INNER JOIN Roles r ON u.RolId = r.Id
WHERE u.Correo IN ('admin@aethereye.com', 'cliente@aethereye.com');

-- Información adicional para debugging
PRINT '✅ Script ejecutado correctamente';
PRINT '📋 Nuevas credenciales de prueba:';
PRINT '   Admin: admin@aethereye.com / AetherAdmin2025!';
PRINT '   Cliente: cliente@aethereye.com / AetherCliente2025!';
PRINT '';
PRINT '🔍 Verificación de datos:';

SELECT 'ROLES EN LA BASE DE DATOS:' AS Informacion;
SELECT Id, Nombre FROM Roles;

SELECT 'USUARIOS CON SUS ROLES:' AS Informacion;
SELECT 
    u.Id,
    u.Correo,
    u.Contrasena,
    r.Nombre AS RolNombre,
    u.EstaActivo
FROM Usuarios u
INNER JOIN Roles r ON u.RolId = r.Id;
