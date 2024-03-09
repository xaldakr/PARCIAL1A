using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<posts> listadopost = (from e in _ParcialContexto.posts
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
            posts? post = (from e in _ParcialContexto.posts
                               where e.id_posts == id
                               select e).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Guardarpost([FromBody] posts post)
        {
            try
            {
                _ParcialContexto.posts.Add(post);
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
        public IActionResult Actualizarpost(int id, [FromBody] posts postModificar)
        {
            //Obtener dato actual
            posts? postActual = (from e in _ParcialContexto.posts
                                     where e.id_posts == id
                                     select e).FirstOrDefault();
            //Verificar su existencia
            if (postActual == null)
            {
                return NotFound();
            }
            //Si se encuentra, modificar
            postActual.nombre = postModificar.nombre;
            postActual.descripcion = postModificar.descripcion;
            postActual.marca_id = postModificar.marca_id;
            postActual.tipo_post_id = postModificar.tipo_post_id;
            postActual.anio_compra = postModificar.anio_compra;
            postActual.costo = postModificar.costo;
            //Marcando como modificado
            _ParcialContexto.Entry(postActual).State = EntityState.Modified;
            _ParcialContexto.SaveChanges();

            return Ok(postModificar);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult Eliminarpost(int id)
        {
            posts? post = (from e in _ParcialContexto.posts
                               where e.id_posts == id
                               select e).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }
            _ParcialContexto.posts.Attach(post);
            _ParcialContexto.posts.Remove(post);
            _ParcialContexto.SaveChanges();
            return Ok(post);
        }
    }
}
