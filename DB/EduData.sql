-----CREAR BASE DE DATOS
CREATE DATABASE EduData;
GO

-----USAR DATABASE
USE EduData;
GO

-----CREAR BASE TABLA DE USUARIOS
CREATE TABLE Usuarios(
Id INT PRIMARY KEY IDENTITY(1,1),
Apodo NVARCHAR(50) UNIQUE NOT NULL,
Correo NVARCHAR(100) UNIQUE NOT NULL,
Clave NVARCHAR(100) UNIQUE NOT NULL,
Estado BIT NOT NULL DEFAULT 1,
Rol NVARCHAR(20) NOT NULL DEFAULT 'Usuario',
Fecha_Creacion DATETIME NOT NULL DEFAULT GETDATE(),
Creado_Por NVARCHAR(20) NOT NULL,
Fecha_Modificacion DATETIME NULL,
Modificado_Por NVARCHAR(50) NULL,
);
GO


-----PROCEDIMIENTOS ALMACENADOS


-- ==============================================
-- PROCEDIMIENTOS PARA GESTIÓN DE USUARIOS
-- ==============================================

------- CREAR USUARIO NUEVO
CREATE PROCEDURE sp_CrearUsuario
    @Apodo NVARCHAR(50),
    @Correo NVARCHAR(100),
    @Clave NVARCHAR(255), -- Ya debe venir hasheada
    @Estado BIT = 1,
    @Rol NVARCHAR(20) = 'Usuario',
    @Creado_Por NVARCHAR(50)
AS
BEGIN
    INSERT INTO Usuarios (
        Apodo,
        Correo,
        Clave,
        Estado,
        Rol,
        Fecha_Creacion,
        Creado_Por
    )
    VALUES (
        @Apodo,
        @Correo,
        @Clave,
        @Estado,
        @Rol,
        GETDATE(),
        @Creado_Por
    );
END
GO


-----ACTUALIZACION DE DATOS DE USUARIO

CREATE PROCEDURE sp_ModificarUsuario
    @Id INT,
    @Apodo NVARCHAR(50),
    @Correo NVARCHAR(100),
    @Estado BIT,
    @Rol NVARCHAR(20),
    @Modificado_Por NVARCHAR(50)
AS
BEGIN
    UPDATE Usuarios
    SET Apodo = @Apodo,
        Correo = @Correo,
        Estado = @Estado,
        Rol = @Rol,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = @Modificado_Por
    WHERE Id = @Id;
END
GO

-------CAMBIO DE CONTRASEÑA
CREATE PROCEDURE sp_CambiarClave
    @Id INT,
    @Nueva_Clave NVARCHAR(255), -- Ya debe venir hasheada
    @Modificado_Por NVARCHAR(50)
AS
BEGIN
    UPDATE Usuarios
    SET Clave = @Nueva_Clave,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = @Modificado_Por
    WHERE Id = @Id;
END
GO

--------------LEER USUARIOS
CREATE PROCEDURE sp_LeerUsuarios
AS
BEGIN
    SELECT Id,
           Apodo,
           Correo,
           Estado,
           Rol,
           Fecha_Creacion,
           Creado_Por,
           Fecha_Modificacion,
           Modificado_Por
    FROM Usuarios;
END
GO


--------BUSCAR POR ID
CREATE PROCEDURE sp_BuscarUsuarioPorId
    @Id INT
AS
BEGIN
    SELECT Id,
           Apodo,
           Correo,
           Clave,
           Estado,
           Rol,
           Fecha_Creacion,
           Creado_Por,
           Fecha_Modificacion,
           Modificado_Por
    FROM Usuarios
    WHERE Id = @Id;
END
GO


-------BUSCAR POR APODO
CREATE PROCEDURE sp_BuscarUsuarioPorApodo
    @Apodo NVARCHAR(50)
AS
BEGIN
    SELECT Id,
           Apodo,
           Correo,
           Clave,
           Estado,
           Rol,
           Fecha_Creacion,
           Creado_Por,
           Fecha_Modificacion,
           Modificado_Por
    FROM Usuarios
    WHERE Apodo LIKE '%' + @Apodo + '%';
END
GO
---ELIMINAR USUARIO 
CREATE PROCEDURE sp_EliminarUsuario
    @Id INT
AS
BEGIN
    DELETE FROM Usuarios
    WHERE Id = @Id;
END
GO