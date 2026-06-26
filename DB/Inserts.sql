------------INSERTS PROFESORES


DECLARE @IdProfesor1 INT, @IdProfesor2 INT, @IdProfesor3 INT;

EXEC sp_CrearProfesor
    'Juan Pérez',
    'juan.perez@edudata.com',
    'Matemáticas',
    'admin',
    @IdProfesor1 OUTPUT;

EXEC sp_CrearProfesor
    'María Gómez',
    'maria.gomez@edudata.com',
    'Programación',
    'admin',
    @IdProfesor2 OUTPUT;

EXEC sp_CrearProfesor
    'Carlos Sánchez',
    'carlos.sanchez@edudata.com',
    'Bases de Datos',
    'admin',
    @IdProfesor3 OUTPUT;

------------INSERTS CURSOS

DECLARE @IdCurso1 INT, @IdCurso2 INT, @IdCurso3 INT;

EXEC sp_CrearCurso
    'Álgebra I',
    'Curso introductorio de álgebra',
    @IdProfesor1,
    'admin',
    @IdCurso1 OUTPUT;

EXEC sp_CrearCurso
    'Programación en C#',
    'Fundamentos de programación orientada a objetos',
    @IdProfesor2,
    'admin',
    @IdCurso2 OUTPUT;

EXEC sp_CrearCurso
    'SQL Server',
    'Diseño y administración de bases de datos',
    @IdProfesor3,
    'admin',
    @IdCurso3 OUTPUT;

------------INSERTS ESTUDIANTES

DECLARE @IdEst1 INT, @IdEst2 INT, @IdEst3 INT;

EXEC sp_CrearEstudiante
    'Ana López',
    'ana.lopez@correo.com',
    '88881111',
    'admin',
    @IdEst1 OUTPUT;

EXEC sp_CrearEstudiante
    'Pedro Ramírez',
    'pedro.ramirez@correo.com',
    '88882222',
    'admin',
    @IdEst2 OUTPUT;

EXEC sp_CrearEstudiante
    'Lucía Fernández',
    'lucia.fernandez@correo.com',
    '88883333',
    'admin',
    @IdEst3 OUTPUT;


---------INSERTS USUARIOS

INSERT INTO Usuarios
(
    Apodo,
    Correo,
    Clave,
    Estado,
    Rol,
    Creado_Por
)
VALUES
('admin1','admin1@edudata.com','Clave123',1,'Administrador','Sistema'),
('docente1','docente1@edudata.com','Clave123',1,'Profesor','Sistema'),
('estudiante1','estudiante1@edudata.com','Clave123',1,'Usuario','Sistema');