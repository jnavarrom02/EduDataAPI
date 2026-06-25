using System.Data;
using Microsoft.Data.SqlClient;
using EduDataAPI.Modelos;

namespace EduDataAPI.Repositorios
{
    public class ProfesorRepository : BaseRepository
    {
        public ProfesorRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CrearProfesor(Profesor profesor)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_CrearProfesor", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Nombre", profesor.Nombre);
            command.Parameters.AddWithValue("@Email", profesor.Email);
            command.Parameters.AddWithValue("@Especialidad", profesor.Especialidad);
            command.Parameters.AddWithValue("@Creado_Por", profesor.Creado_Por);
            command.Parameters.Add("@NuevoId", SqlDbType.Int).Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)command.Parameters["@NuevoId"].Value!;
        }

        public async Task<List<Profesor>> LeerProfesores()
        {
            var profesores = new List<Profesor>();

            using var connection = GetConnection();
            using var command = new SqlCommand("sp_LeerProfesores", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                profesores.Add(new Profesor
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = Convert.ToString(row["Nombre"]) ?? string.Empty,
                    Email = Convert.ToString(row["Email"]) ?? string.Empty,
                    Especialidad = Convert.ToString(row["Especialidad"]) ?? string.Empty,
                    Activo = Convert.ToBoolean(row["Activo"]),
                    Fecha_Creacion = Convert.ToDateTime(row["Fecha_Creacion"]),
                    Creado_Por = Convert.ToString(row["Creado_Por"]) ?? string.Empty,
                    Fecha_Modificacion = row.IsNull("Fecha_Modificacion") ? null : Convert.ToDateTime(row["Fecha_Modificacion"]),
                    Modificado_Por = row.IsNull("Modificado_Por") ? null : Convert.ToString(row["Modificado_Por"])
                });
            }

            return profesores;
        }

        public async Task<Profesor?> BuscarProfesorPorId(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_BuscarProfesorPorId", connection)
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

            return new Profesor
            {
                Id = reader.GetInt32("Id"),
                Nombre = reader.GetString("Nombre"),
                Email = reader.GetString("Email"),
                Especialidad = reader.GetString("Especialidad"),
                Activo = reader.GetBoolean("Activo"),
                Fecha_Creacion = reader.GetDateTime("Fecha_Creacion"),
                Creado_Por = reader.GetString("Creado_Por"),
                Fecha_Modificacion = reader.IsDBNull("Fecha_Modificacion") ? null : reader.GetDateTime("Fecha_Modificacion"),
                Modificado_Por = reader.IsDBNull("Modificado_Por") ? null : reader.GetString("Modificado_Por")
            };
        }

        public async Task ModificarProfesor(Profesor profesor)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_ModificarProfesor", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", profesor.Id);
            command.Parameters.AddWithValue("@Nombre", profesor.Nombre);
            command.Parameters.AddWithValue("@Email", profesor.Email);
            command.Parameters.AddWithValue("@Especialidad", profesor.Especialidad);
            command.Parameters.AddWithValue("@Modificado_Por", profesor.Modificado_Por ?? string.Empty);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task EliminarProfesor(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_EliminarProfesor", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
