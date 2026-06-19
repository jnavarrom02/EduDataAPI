using EduDataAPI.Modelos;
using EduDataAPI.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace EduDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repository;

        public UsuarioController(UsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearUsuario([FromBody] Usuario usuario)
        {
            await _repository.CrearUsuario(usuario);

            return CreatedAtAction(
                nameof(BuscarUsuarioPorId),
                new { id = usuario.Id },
                usuario);
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> LeerUsuarios()
        {
            return await _repository.LeerUsuarios();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModificarUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            await _repository.ModificarUsuario(usuario);

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> EliminarUsuario(int id)
        //{
        //    await _repository.EliminarUsuario(id);

        //    return NoContent();
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> BuscarUsuarioPorId(int id)
        {
            var usuario = await _repository.BuscarUsuarioPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
    }
}