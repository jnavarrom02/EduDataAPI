-----CREAR BASE DE DATOS
IF DB_ID('EduData') IS NULL
BEGIN
    CREATE DATABASE EduData;
END
GO

-----USAR BASE DE DATOS
USE EduData;
GO

-----TABLA DE USUARIOS

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


-----TABLA DE PROFESORES
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Profesores' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Profesores
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Especialidad NVARCHAR(100) NOT NULL,
        Activo BIT NOT NULL DEFAULT 1,
        Fecha_Creacion DATETIME NOT NULL DEFAULT GETDATE(),
        Creado_Por NVARCHAR(50) NOT NULL,
        Fecha_Modificacion DATETIME NULL,
        Modificado_Por NVARCHAR(50) NULL
    );
END
GO

-----TABLA DE CURSOS
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Cursos' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Cursos
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        IdProfesor INT NOT NULL,
        Activo BIT NOT NULL DEFAULT 1,
        Fecha_Creacion DATETIME NOT NULL DEFAULT GETDATE(),
        Creado_Por NVARCHAR(50) NOT NULL,
        Fecha_Modificacion DATETIME NULL,
        Modificado_Por NVARCHAR(50) NULL,
        CONSTRAINT FK_Cursos_Profesores FOREIGN KEY (IdProfesor) REFERENCES Profesores(Id)
    );
END
GO
CREATE INDEX IX_Cursos_IdProfesor ON Cursos(IdProfesor);
GO

-----TABLA DE ESTUDIANTES
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Estudiantes' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Estudiantes
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Telefono NVARCHAR(20) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        Fecha_Creacion DATETIME NOT NULL DEFAULT GETDATE(),
        Creado_Por NVARCHAR(50) NOT NULL,
        Fecha_Modificacion DATETIME NULL,
        Modificado_Por NVARCHAR(50) NULL
    );
END
GO

-----TABLA INTERMEDIA ESTUDIANTE-CURSO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'EstudianteCursos' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE EstudianteCursos
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdEstudiante INT NOT NULL,
        IdCurso INT NOT NULL,
        Activo BIT NOT NULL DEFAULT 1,
        Fecha_Creacion DATETIME NOT NULL DEFAULT GETDATE(),
        Creado_Por NVARCHAR(50) NOT NULL,
        Fecha_Modificacion DATETIME NULL,
        Modificado_Por NVARCHAR(50) NULL,
        CONSTRAINT FK_EstudianteCursos_Estudiantes FOREIGN KEY (IdEstudiante) REFERENCES Estudiantes(Id),
        CONSTRAINT FK_EstudianteCursos_Cursos FOREIGN KEY (IdCurso) REFERENCES Cursos(Id)
    );
END
GO
CREATE INDEX IX_EstudianteCursos_IdEstudiante ON EstudianteCursos(IdEstudiante);
GO
CREATE INDEX IX_EstudianteCursos_IdCurso ON EstudianteCursos(IdCurso);
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

ALTER PROCEDURE sp_LeerUsuarios
AS
BEGIN
    SELECT
           Id,
           Apodo,
           Correo,
           Clave,
           Estado,
           Rol,
           Fecha_Creacion,
           Creado_Por,
           Fecha_Modificacion,
           Modificado_Por
    FROM Usuarios;
END
GO

-----PROCEDIMIENTOS PARA PROFESORES
CREATE PROCEDURE sp_CrearProfesor
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Especialidad NVARCHAR(100),
    @Creado_Por NVARCHAR(50),
    @NuevoId INT OUTPUT
AS
BEGIN
    INSERT INTO Profesores (Nombre, Email, Especialidad, Activo, Fecha_Creacion, Creado_Por)
    VALUES (@Nombre, @Email, @Especialidad, 1, GETDATE(), @Creado_Por);
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_LeerProfesores
AS
BEGIN
    SELECT Id, Nombre, Email, Especialidad, Activo, Fecha_Creacion, Creado_Por, Fecha_Modificacion, Modificado_Por
    FROM Profesores
    WHERE Activo = 1;
END
GO

CREATE PROCEDURE sp_BuscarProfesorPorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Email, Especialidad, Activo, Fecha_Creacion, Creado_Por, Fecha_Modificacion, Modificado_Por
    FROM Profesores
    WHERE Id = @Id AND Activo = 1;
END
GO

CREATE PROCEDURE sp_ModificarProfesor
    @Id INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Especialidad NVARCHAR(100),
    @Modificado_Por NVARCHAR(50)
AS
BEGIN
    UPDATE Profesores
    SET Nombre = @Nombre,
        Email = @Email,
        Especialidad = @Especialidad,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = @Modificado_Por
    WHERE Id = @Id AND Activo = 1;
END
GO

CREATE PROCEDURE sp_EliminarProfesor
    @Id INT
AS
BEGIN
    UPDATE Profesores
    SET Activo = 0,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = 'Sistema'
    WHERE Id = @Id;
END
GO

-----PROCEDIMIENTOS PARA CURSOS
CREATE PROCEDURE sp_CrearCurso
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(500),
    @IdProfesor INT,
    @Creado_Por NVARCHAR(50),
    @NuevoId INT OUTPUT
AS
BEGIN
    INSERT INTO Cursos (Nombre, Descripcion, IdProfesor, Activo, Fecha_Creacion, Creado_Por)
    VALUES (@Nombre, @Descripcion, @IdProfesor, 1, GETDATE(), @Creado_Por);
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_LeerCursos
AS
BEGIN
    SELECT Id, Nombre, Descripcion, IdProfesor, Activo, Fecha_Creacion, Creado_Por, Fecha_Modificacion, Modificado_Por
    FROM Cursos
    WHERE Activo = 1;
END
GO

CREATE PROCEDURE sp_BuscarCursoPorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Descripcion, IdProfesor, Activo, Fecha_Creacion, Creado_Por, Fecha_Modificacion, Modificado_Por
    FROM Cursos
    WHERE Id = @Id AND Activo = 1;
END
GO

CREATE PROCEDURE sp_ModificarCurso
    @Id INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(500),
    @IdProfesor INT,
    @Modificado_Por NVARCHAR(50)
AS
BEGIN
    UPDATE Cursos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        IdProfesor = @IdProfesor,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = @Modificado_Por
    WHERE Id = @Id AND Activo = 1;
END
GO

CREATE PROCEDURE sp_EliminarCurso
    @Id INT
AS
BEGIN
    UPDATE Cursos
    SET Activo = 0,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = 'Sistema'
    WHERE Id = @Id;
END
GO

-----PROCEDIMIENTOS PARA ESTUDIANTES
CREATE PROCEDURE sp_CrearEstudiante
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Creado_Por NVARCHAR(50),
    @NuevoId INT OUTPUT
AS
BEGIN
    INSERT INTO Estudiantes (Nombre, Email, Telefono, Activo, Fecha_Creacion, Creado_Por)
    VALUES (@Nombre, @Email, @Telefono, 1, GETDATE(), @Creado_Por);
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_LeerEstudiantes
AS
BEGIN
    SELECT Id, Nombre, Email, Telefono, Activo, Fecha_Creacion, Creado_Por, Fecha_Modificacion, Modificado_Por
    FROM Estudiantes
    WHERE Activo = 1;
END
GO

CREATE PROCEDURE sp_BuscarEstudiantePorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Email, Telefono, Activo, Fecha_Creacion, Creado_Por, Fecha_Modificacion, Modificado_Por
    FROM Estudiantes
    WHERE Id = @Id AND Activo = 1;
END
GO

CREATE PROCEDURE sp_ModificarEstudiante
    @Id INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Modificado_Por NVARCHAR(50)
AS
BEGIN
    UPDATE Estudiantes
    SET Nombre = @Nombre,
        Email = @Email,
        Telefono = @Telefono,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = @Modificado_Por
    WHERE Id = @Id AND Activo = 1;
END
GO

CREATE PROCEDURE sp_EliminarEstudiante
    @Id INT
AS
BEGIN
    UPDATE Estudiantes
    SET Activo = 0,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = 'Sistema'
    WHERE Id = @Id;
END
GO

-----PROCEDIMIENTOS PARA ESTUDIANTE-CURSO
CREATE PROCEDURE sp_CrearEstudianteCurso
    @IdEstudiante INT,
    @IdCurso INT,
    @Creado_Por NVARCHAR(50),
    @NuevoId INT OUTPUT
AS
BEGIN
    INSERT INTO EstudianteCursos (IdEstudiante, IdCurso, Activo, Fecha_Creacion, Creado_Por)
    VALUES (@IdEstudiante, @IdCurso, 1, GETDATE(), @Creado_Por);
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_EliminarEstudianteCurso
    @Id INT
AS
BEGIN
    UPDATE EstudianteCursos
    SET Activo = 0,
        Fecha_Modificacion = GETDATE(),
        Modificado_Por = 'Sistema'
    WHERE Id = @Id;
END
GO
