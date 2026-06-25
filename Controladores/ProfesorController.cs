using EduDataAPI.Modelos;
using EduDataAPI.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace EduDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly ProfesorRepository _repository;

        public ProfesorController(ProfesorRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Profesor>> CrearProfesor([FromBody] Profesor profesor)
        {
            try
            {
                var id = await _repository.CrearProfesor(profesor);
                profesor.Id = id;
                return CreatedAtAction(nameof(ObtenerProfesorPorId), new { id }, profesor);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Profesor>>> ListarProfesores()
        {
            try
            {
                return await _repository.LeerProfesores();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profesor>> ObtenerProfesorPorId(int id)
        {
            try
            {
                var profesor = await _repository.BuscarProfesorPorId(id);
                if (profesor == null)
                {
                    return NotFound();
                }
                return profesor;
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarProfesor(int id)
        {
            try
            {
                await _repository.EliminarProfesor(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
    }
}
