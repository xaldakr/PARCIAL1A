using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL1A.Models;

namespace PARCIAL1A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorLibroLibroController : ControllerBase
    {
        private readonly ParcialContext _ParcialContexto;

        public AutorLibroLibroController(ParcialContext ParcialContexto)
        {
            _ParcialContexto = ParcialContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Getto()
        {
            List<autorlibro> listadoautorlibro = (from e in _ParcialContexto.AutorLibro
                                        select e).ToList();
            if (listadoautorlibro.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoautorlibro);
        }

        [HttpGet]
        [Route("GetByIdLibro/{id}")]
        public IActionResult GetLib(int id)
        {
            autorlibro? autorlibro = (from e in _ParcialContexto.AutorLibro
                            where e.LibroId == id
                            select e).FirstOrDefault();
            if (autorlibro == null)
            {
                return NotFound();
            }
            return Ok(autorlibro);
        }
        [HttpGet]
        [Route("GetByIdAutor/{id}")]
        public IActionResult GetAutor(int id)
        {
            autorlibro? autorlibro = (from e in _ParcialContexto.AutorLibro
                                      where e.AutorId == id
                                      select e).FirstOrDefault();
            if (autorlibro == null)
            {
                return NotFound();
            }
            return Ok(autorlibro);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Guardarautorlibro([FromBody] autorlibro autorlibro)
        {
            try
            {
                _ParcialContexto.AutorLibro.Add(autorlibro);
                _ParcialContexto.SaveChanges();
                return Ok(autorlibro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ActualizarLib/{id}")]
        public IActionResult ActualizarautorlibroLib(int id, [FromBody] autorlibro autorlibroModificar)
        {
            //Obtener dato actual
            autorlibro? autorlibroActual = (from e in _ParcialContexto.AutorLibro
                                  where e.LibroId == id
                                  select e).FirstOrDefault();
            //Verificar su existencia
            if (autorlibroActual == null)
            {
                return NotFound();
            }
            //Si se encuentra, modificar
            autorlibroActual.Orden = autorlibroModificar.Orden;

            //Marcando como modificado
            _ParcialContexto.Entry(autorlibroActual).State = EntityState.Modified;
            _ParcialContexto.SaveChanges();

            return Ok(autorlibroModificar);
        }

        [HttpPut]
        [Route("ActualizarAut/{id}")]
        public IActionResult ActualizarautorlibroAut(int id, [FromBody] autorlibro autorlibroModificar)
        {
            //Obtener dato actual
            autorlibro? autorlibroActual = (from e in _ParcialContexto.AutorLibro
                                            where e.AutorId == id
                                            select e).FirstOrDefault();
            //Verificar su existencia
            if (autorlibroActual == null)
            {
                return NotFound();
            }
            //Si se encuentra, modificar
            autorlibroActual.Orden = autorlibroModificar.Orden;

            //Marcando como modificado
            _ParcialContexto.Entry(autorlibroActual).State = EntityState.Modified;
            _ParcialContexto.SaveChanges();

            return Ok(autorlibroModificar);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult Eliminarautorlibro(int id)
        {
            autorlibro? autorlibro = (from e in _ParcialContexto.AutorLibro
                            where e.Id == id
                            select e).FirstOrDefault();
            if (autorlibro == null)
            {
                return NotFound();
            }
            _ParcialContexto.AutorLibro.Attach(autorlibro);
            _ParcialContexto.AutorLibro.Remove(autorlibro);
            _ParcialContexto.SaveChanges();
            return Ok(autorlibro);
        }
    }
}
