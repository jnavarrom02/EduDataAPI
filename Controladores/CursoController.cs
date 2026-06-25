using EduDataAPI.Modelos;
using EduDataAPI.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace EduDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly CursoRepository _repository;

        public CursoController(CursoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Curso>> CrearCurso([FromBody] Curso curso)
        {
            try
            {
                var id = await _repository.CrearCurso(curso);
                curso.Id = id;
                return CreatedAtAction(nameof(ObtenerCursoPorId), new { id }, curso);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> ListarCursos()
        {
            try
            {
                return await _repository.LeerCursos();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> ObtenerCursoPorId(int id)
        {
            try
            {
                var curso = await _repository.BuscarCursoPorId(id);
                if (curso == null)
                {
                    return NotFound();
                }
                return curso;
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModificarCurso(int id, [FromBody] Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest("El id de la ruta debe coincidir con el id del curso.");
            }

            try
            {
                await _repository.ModificarCurso(curso);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCurso(int id)
        {
            try
            {
                await _repository.EliminarCurso(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
    }
}
