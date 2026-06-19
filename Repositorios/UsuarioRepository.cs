using EduDataAPI.Modelos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EduDataAPI.Repositorios
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task CrearUsuario(Usuario usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@Apodo", usuario.Apodo),
                new SqlParameter("@Correo", usuario.Correo),
                new SqlParameter("@Clave", usuario.Clave),
                new SqlParameter("@Estado", usuario.Estado),
                new SqlParameter("@Rol", usuario.Rol),
                new SqlParameter("@Creado_Por", usuario.Creado_Por)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CrearUsuario @Apodo, @Correo, @Clave, @Estado, @Rol, @Creado_Por",
                parametros);
        }


        public async Task ModificarUsuario(Usuario usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@Id", usuario.Id),
                new SqlParameter("@Apodo", usuario.Apodo),
                new SqlParameter("@Correo", usuario.Correo),
                new SqlParameter("@Estado", usuario.Estado),
                new SqlParameter("@Rol", usuario.Rol),
                new SqlParameter("@Modificado_Por",
                    usuario.Modificado_Por ?? string.Empty)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ModificarUsuario @Id, @Apodo, @Correo, @Estado, @Rol, @Modificado_Por",
                parametros);
        }


        public async Task CambiarClave(
            int id,
            string nuevaClave,
            string modificadoPor)
        {
            var parametros = new[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nueva_Clave", nuevaClave),
                new SqlParameter("@Modificado_Por", modificadoPor)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CambiarClave @Id, @Nueva_Clave, @Modificado_Por",
                parametros);
        }


        public async Task<List<Usuario>> LeerUsuarios()
        {
            return await _context.Usuarios
                .FromSqlRaw("EXEC sp_LeerUsuarios")
                .ToListAsync();
        }

 


        public async Task<Usuario?> BuscarUsuarioPorId(int id)
        {
            var parametro = new SqlParameter("@Id", id);

            return await _context.Usuarios
                .FromSqlRaw("EXEC sp_BuscarUsuarioPorId @Id", parametro)
                .FirstOrDefaultAsync();
        }





        public async Task<List<Usuario>> BuscarUsuarioPorApodo(string apodo)
        {
            var parametro = new SqlParameter("@Apodo", apodo);

            return await _context.Usuarios
                .FromSqlRaw("EXEC sp_BuscarUsuarioPorApodo @Apodo", parametro)
                .ToListAsync();
        }

        public async Task EliminarUsuario(int id)
        {
            var parametro = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_EliminarUsuario @Id",
                parametro);
        }


    }
}