using System.Data;
using Microsoft.Data.SqlClient;
using EduDataAPI.Modelos;

namespace EduDataAPI.Repositorios
{
    public class EstudianteRepository : BaseRepository
    {
        public EstudianteRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CrearEstudiante(Estudiante estudiante)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_CrearEstudiante", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
            command.Parameters.AddWithValue("@Email", estudiante.Email);
            command.Parameters.AddWithValue("@Telefono", estudiante.Telefono);
            command.Parameters.AddWithValue("@Creado_Por", estudiante.Creado_Por);
            command.Parameters.Add("@NuevoId", SqlDbType.Int).Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)command.Parameters["@NuevoId"].Value!;
        }

        public async Task<List<Estudiante>> LeerEstudiantes()
        {
            var estudiantes = new List<Estudiante>();

            using var connection = GetConnection();
            using var command = new SqlCommand("sp_LeerEstudiantes", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                estudiantes.Add(new Estudiante
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Email = reader.GetString("Email"),
                    Telefono = reader.GetString("Telefono"),
                    Activo = reader.GetBoolean("Activo"),
                    Fecha_Creacion = reader.GetDateTime("Fecha_Creacion"),
                    Creado_Por = reader.GetString("Creado_Por"),
                    Fecha_Modificacion = reader.IsDBNull("Fecha_Modificacion") ? null : reader.GetDateTime("Fecha_Modificacion"),
                    Modificado_Por = reader.IsDBNull("Modificado_Por") ? null : reader.GetString("Modificado_Por")
                });
            }

            return estudiantes;
        }

        public async Task<Estudiante?> BuscarEstudiantePorId(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_BuscarEstudiantePorId", connection)
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

            return new Estudiante
            {
                Id = reader.GetInt32("Id"),
                Nombre = reader.GetString("Nombre"),
                Email = reader.GetString("Email"),
                Telefono = reader.GetString("Telefono"),
                Activo = reader.GetBoolean("Activo"),
                Fecha_Creacion = reader.GetDateTime("Fecha_Creacion"),
                Creado_Por = reader.GetString("Creado_Por"),
                Fecha_Modificacion = reader.IsDBNull("Fecha_Modificacion") ? null : reader.GetDateTime("Fecha_Modificacion"),
                Modificado_Por = reader.IsDBNull("Modificado_Por") ? null : reader.GetString("Modificado_Por")
            };
        }

        public async Task ModificarEstudiante(Estudiante estudiante)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_ModificarEstudiante", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", estudiante.Id);
            command.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
            command.Parameters.AddWithValue("@Email", estudiante.Email);
            command.Parameters.AddWithValue("@Telefono", estudiante.Telefono);
            command.Parameters.AddWithValue("@Modificado_Por", estudiante.Modificado_Por ?? string.Empty);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task EliminarEstudiante(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_EliminarEstudiante", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
