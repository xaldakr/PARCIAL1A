using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PARCIAL1A.Models;

namespace PARCIAL1A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ParcialContext _ParcialContexto;

        public PostController(ParcialContext ParcialContexto)
        {
            _ParcialContexto = ParcialContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Getto()
        {
            List<post> listadopost = (from e in _ParcialContexto.Posts
                                      select e).ToList();
            if (listadopost.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadopost);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            post? post = (from e in _ParcialContexto.Posts
                          where e.Id == id
                           select e).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Guardarpost([FromBody] post post)
        {
            try
            {
                _ParcialContexto.Posts.Add(post);
                _ParcialContexto.SaveChanges();
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult Actualizarpost(int id, [FromBody] post postModificar)
        {
            //Obtener dato actual
            post? postActual = (from e in _ParcialContexto.Posts
                                where e.Id == id
                                 select e).FirstOrDefault();
            //Verificar su existencia
            if (postActual == null)
            {
                return NotFound();
            }
            //Si se encuentra, modificar
            postActual.Titulo = postModificar.Titulo;
            postActual.Contenido = postModificar.Contenido;
            postActual.FechaPublicacion = postModificar.FechaPublicacion;
            postActual.AutorId = postModificar.AutorId;
            //Marcando como modificado
            _ParcialContexto.Entry(postActual).State = EntityState.Modified;
            _ParcialContexto.SaveChanges();

            return Ok(postModificar);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult Eliminarpost(int id)
        {
            post? post = (from e in _ParcialContexto.Posts
                          where e.Id == id
                           select e).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            _ParcialContexto.Posts.Attach(post);
            _ParcialContexto.Posts.Remove(post);
            _ParcialContexto.SaveChanges();
            return Ok(post);
        }
    }
}
