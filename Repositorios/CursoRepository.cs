using System.Data;
using Microsoft.Data.SqlClient;
using EduDataAPI.Modelos;

namespace EduDataAPI.Repositorios
{
    public class CursoRepository : BaseRepository
    {
        public CursoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CrearCurso(Curso curso)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_CrearCurso", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Nombre", curso.Nombre);
            command.Parameters.AddWithValue("@Descripcion", curso.Descripcion);
            command.Parameters.AddWithValue("@IdProfesor", curso.IdProfesor);
            command.Parameters.AddWithValue("@Creado_Por", curso.Creado_Por);
            command.Parameters.Add("@NuevoId", SqlDbType.Int).Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)command.Parameters["@NuevoId"].Value!;
        }

        public async Task<List<Curso>> LeerCursos()
        {
            var cursos = new List<Curso>();

            using var connection = GetConnection();
            using var command = new SqlCommand("sp_LeerCursos", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cursos.Add(new Curso
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Descripcion = reader.GetString("Descripcion"),
                    IdProfesor = reader.GetInt32("IdProfesor"),
                    Activo = reader.GetBoolean("Activo"),
                    Fecha_Creacion = reader.GetDateTime("Fecha_Creacion"),
                    Creado_Por = reader.GetString("Creado_Por"),
                    Fecha_Modificacion = reader.IsDBNull("Fecha_Modificacion") ? null : reader.GetDateTime("Fecha_Modificacion"),
                    Modificado_Por = reader.IsDBNull("Modificado_Por") ? null : reader.GetString("Modificado_Por")
                });
            }

            return cursos;
        }

        public async Task<Curso?> BuscarCursoPorId(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_BuscarCursoPorId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new Curso
            {
                Id = reader.GetInt32("Id"),
                Nombre = reader.GetString("Nombre"),
                Descripcion = reader.GetString("Descripcion"),
                IdProfesor = reader.GetInt32("IdProfesor"),
                Activo = reader.GetBoolean("Activo"),
                Fecha_Creacion = reader.GetDateTime("Fecha_Creacion"),
                Creado_Por = reader.GetString("Creado_Por"),
                Fecha_Modificacion = reader.IsDBNull("Fecha_Modificacion") ? null : reader.GetDateTime("Fecha_Modificacion"),
                Modificado_Por = reader.IsDBNull("Modificado_Por") ? null : reader.GetString("Modificado_Por")
            };
        }

        public async Task ModificarCurso(Curso curso)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_ModificarCurso", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", curso.Id);
            command.Parameters.AddWithValue("@Nombre", curso.Nombre);
            command.Parameters.AddWithValue("@Descripcion", curso.Descripcion);
            command.Parameters.AddWithValue("@IdProfesor", curso.IdProfesor);
            command.Parameters.AddWithValue("@Modificado_Por", curso.Modificado_Por ?? string.Empty);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task EliminarCurso(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_EliminarCurso", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
