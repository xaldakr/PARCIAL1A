using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL1A.Models;

namespace PARCIAL1A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly ParcialContext _ParcialContexto;

        public AutorController(ParcialContext ParcialContexto)
        {
            _ParcialContexto = ParcialContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Getto()
        {
            List<autor> listadoautor = (from e in _ParcialContexto.Autores
                                      select e).ToList();
            if (listadoautor.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoautor);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            autor? autor = (from e in _ParcialContexto.Autores
                            where e.Id == id
                          select e).FirstOrDefault();
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Guardarautor([FromBody] autor autor)
        {
            try
            {
                _ParcialContexto.Autores.Add(autor);
                _ParcialContexto.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult Actualizarautor(int id, [FromBody] autor autorModificar)
        {
            //Obtener dato actual
            autor? autorActual = (from e in _ParcialContexto.Autores
                                  where e.Id == id
                                select e).FirstOrDefault();
            //Verificar su existencia
            if (autorActual == null)
            {
                return NotFound();
            }
            //Si se encuentra, modificar
            autorActual.Nombre = autorModificar.Nombre;
            //Marcando como modificado
            _ParcialContexto.Entry(autorActual).State = EntityState.Modified;
            _ParcialContexto.SaveChanges();

            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult Eliminarautor(int id)
        {
            autor? autor = (from e in _ParcialContexto.Autores
                            where e.Id == id
                          select e).FirstOrDefault();
            if (autor == null)
            {
                return NotFound();
            }
            _ParcialContexto.Autores.Attach(autor);
            _ParcialContexto.Autores.Remove(autor);
            _ParcialContexto.SaveChanges();
            return Ok(autor);
        }
    }
}
