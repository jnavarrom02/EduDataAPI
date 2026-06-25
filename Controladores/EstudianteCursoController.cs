using EduDataAPI.Modelos;
using EduDataAPI.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace EduDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteCursoController : ControllerBase
    {
        private readonly EstudianteCursoRepository _repository;

        public EstudianteCursoController(EstudianteCursoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<EstudianteCurso>> InscribirEstudiante([FromBody] EstudianteCurso inscripcion)
        {
            try
            {
                var id = await _repository.CrearInscripcion(inscripcion);
                inscripcion.Id = id;
                return Created(string.Empty, inscripcion);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarInscripcion(int id)
        {
            try
            {
                var removed = await _repository.EliminarInscripcion(id);
                if (!removed)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
    }
}
