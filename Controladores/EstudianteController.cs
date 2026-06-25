using EduDataAPI.Modelos;
using EduDataAPI.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EduDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly EstudianteRepository _repository;

        public EstudianteController(EstudianteRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Estudiante>> CrearEstudiante([FromBody] Estudiante estudiante)
        {
            try
            {
                var id = await _repository.CrearEstudiante(estudiante);
                estudiante.Id = id;
                return CreatedAtAction(nameof(ObtenerEstudiantePorId), new { id }, estudiante);
            }
            catch (SqlException sqlEx)
            {
                return Problem(sqlEx.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Estudiante>>> ListarEstudiantes()
        {
            try
            {
                return await _repository.LeerEstudiantes();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> ObtenerEstudiantePorId(int id)
        {
            try
            {
                var estudiante = await _repository.BuscarEstudiantePorId(id);
                if (estudiante == null)
                {
                    return NotFound();
                }

                return estudiante;
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModificarEstudiante(int id, [FromBody] Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return BadRequest("El id de la ruta debe coincidir con el id del estudiante.");
            }

            try
            {
                await _repository.ModificarEstudiante(estudiante);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarEstudiante(int id)
        {
            try
            {
                await _repository.EliminarEstudiante(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
    }
}
