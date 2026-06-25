using System.Data;
using Microsoft.Data.SqlClient;
using EduDataAPI.Modelos;

namespace EduDataAPI.Repositorios
{
    public class EstudianteCursoRepository : BaseRepository
    {
        public EstudianteCursoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CrearInscripcion(EstudianteCurso inscripcion)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_CrearEstudianteCurso", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IdEstudiante", inscripcion.IdEstudiante);
            command.Parameters.AddWithValue("@IdCurso", inscripcion.IdCurso);
            command.Parameters.AddWithValue("@Creado_Por", inscripcion.Creado_Por);
            command.Parameters.Add("@NuevoId", SqlDbType.Int).Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)command.Parameters["@NuevoId"].Value!;
        }

        public async Task<bool> EliminarInscripcion(int id)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand("sp_EliminarEstudianteCurso", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}
