# Documentación del proyecto EduDataAPI

## Base de datos

La base de datos `EduData` contiene las siguientes tablas normalizadas:

- `Estudiantes`
- `Profesores`
- `Cursos`
- `EstudianteCursos`

### Relaciones

- Un `Profesor` puede impartir muchos `Cursos`.
- Un `Estudiante` puede inscribirse en muchos `Cursos` mediante la tabla intermedia `EstudianteCursos`.
- `Cursos.IdProfesor` es una llave foránea hacia `Profesores.Id`.
- `EstudianteCursos.IdEstudiante` es una llave foránea hacia `Estudiantes.Id`.
- `EstudianteCursos.IdCurso` es una llave foránea hacia `Cursos.Id`.

### Registros activos

Todas las consultas de lectura sólo devuelven registros con `Activo = 1`.
Los métodos `DELETE` hacen eliminación lógica estableciendo `Activo = 0`.

## Diccionario de datos

### Estudiantes
- `Id`: INT IDENTITY(1,1), PK
- `Nombre`: NVARCHAR(100) NOT NULL
- `Email`: NVARCHAR(100) NOT NULL UNIQUE
- `Telefono`: NVARCHAR(20) NULL
- `Activo`: BIT NOT NULL DEFAULT 1
- `Fecha_Creacion`: DATETIME NOT NULL DEFAULT GETDATE()
- `Creado_Por`: NVARCHAR(50) NOT NULL
- `Fecha_Modificacion`: DATETIME NULL
- `Modificado_Por`: NVARCHAR(50) NULL

### Profesores
- `Id`: INT IDENTITY(1,1), PK
- `Nombre`: NVARCHAR(100) NOT NULL
- `Email`: NVARCHAR(100) NOT NULL UNIQUE
- `Especialidad`: NVARCHAR(100) NOT NULL
- `Activo`: BIT NOT NULL DEFAULT 1
- `Fecha_Creacion`: DATETIME NOT NULL DEFAULT GETDATE()
- `Creado_Por`: NVARCHAR(50) NOT NULL
- `Fecha_Modificacion`: DATETIME NULL
- `Modificado_Por`: NVARCHAR(50) NULL

### Cursos
- `Id`: INT IDENTITY(1,1), PK
- `Nombre`: NVARCHAR(100) NOT NULL
- `Descripcion`: NVARCHAR(500) NULL
- `IdProfesor`: INT NOT NULL, FK -> Profesores(Id)
- `Activo`: BIT NOT NULL DEFAULT 1
- `Fecha_Creacion`: DATETIME NOT NULL DEFAULT GETDATE()
- `Creado_Por`: NVARCHAR(50) NOT NULL
- `Fecha_Modificacion`: DATETIME NULL
- `Modificado_Por`: NVARCHAR(50) NULL

### EstudianteCursos
- `Id`: INT IDENTITY(1,1), PK
- `IdEstudiante`: INT NOT NULL, FK -> Estudiantes(Id)
- `IdCurso`: INT NOT NULL, FK -> Cursos(Id)
- `Activo`: BIT NOT NULL DEFAULT 1
- `Fecha_Creacion`: DATETIME NOT NULL DEFAULT GETDATE()
- `Creado_Por`: NVARCHAR(50) NOT NULL
- `Fecha_Modificacion`: DATETIME NULL
- `Modificado_Por`: NVARCHAR(50) NULL

## Endpoints de la API

### Estudiante
- `POST /api/estudiante` - Crear estudiante
- `GET /api/estudiante` - Listar estudiantes activos
- `GET /api/estudiante/{id}` - Obtener estudiante activo por id
- `PUT /api/estudiante/{id}` - Actualizar estudiante
- `DELETE /api/estudiante/{id}` - Eliminar lógicamente estudiante

### Profesor
- `POST /api/profesor` - Crear profesor
- `GET /api/profesor` - Listar profesores activos
- `GET /api/profesor/{id}` - Obtener profesor activo por id

### Curso
- `POST /api/curso` - Crear curso
- `GET /api/curso` - Listar cursos activos
- `GET /api/curso/{id}` - Obtener curso activo por id
- `PUT /api/curso/{id}` - Actualizar curso

### EstudianteCurso
- `POST /api/estudianteCurso` - Inscribir estudiante en curso
- `DELETE /api/estudianteCurso/{id}` - Eliminar inscripción (lógica)

## Uso de ADO.NET

Los repositorios usan exclusivamente `SqlConnection`, `SqlCommand`, `SqlDataReader` y `SqlDataAdapter`.

## Diagrama ER

El diagrama ER se encuentra en `ER_DIAGRAMA.md`.

## Reglas de negocio clave

- Solo se listan registros con `Activo = 1`.
- Las eliminaciones (`DELETE`) son lógicas y mantienen historial en la tabla.
- `Cursos.IdProfesor` debe existir en `Profesores`.
- `EstudianteCursos` vincula estudiantes y cursos mediante relaciones muchos a muchos.

## Swagger

Al ejecutar la aplicación en modo desarrollo, Swagger está disponible en:

- `/swagger`

## Conexión

La cadena de conexión está definida en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EduData;Integrated Security=True;TrustServerCertificate=True;"
}
```
