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
            var listadopost = (from e in _ParcialContexto.Posts
                                      join a in _ParcialContexto.Autores
                                         on e.AutorId equals a.Id
                                      select new
                                      {
                                          e.Id,
                                          e.Titulo,
                                          e.Contenido,
                                          e.FechaPublicacion,
                                          a.Nombre
                                      }).ToList();
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
            var post = (from e in _ParcialContexto.Posts
                        join a in _ParcialContexto.Autores
                           on e.AutorId equals a.Id
                        where e.Id == id
                        select new
                        {
                            e.Id,
                            e.Titulo,
                            e.Contenido,
                            e.FechaPublicacion,
                            a.Nombre
                        }).FirstOrDefault();
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
        [HttpGet]
        [Route("ListaPostbyAutor/{autor}")]
        public IActionResult Findautor(string autor)
        {
            var listaPost = (from e in _ParcialContexto.Posts
                             join a in _ParcialContexto.Autores
                                on e.AutorId equals a.Id
                             where a.Nombre.Contains(autor)
                             select new
                             {
                                 e.Id,
                                 e.Titulo,
                                 e.Contenido,
                                 e.FechaPublicacion,
                                 a.Nombre
                             }).OrderBy(result => result.FechaPublicacion).Take(20).ToList();
            if (listaPost.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listaPost);
        }
        [HttpGet]
        [Route("ListaPostbyLibro/{libro}")]
        public IActionResult Findlibro(string libro)
        {
            var listaPost = (from e in _ParcialContexto.Libros
                             join ae in _ParcialContexto.AutorLibro
                                on e.Id equals ae.LibroId
                             join a in _ParcialContexto.Autores
                                on ae.AutorId equals a.Id
                             join tot in _ParcialContexto.Posts
                                on a.Id equals tot.AutorId
                             where e.Titulo.Contains(libro)
                             select new
                             {
                                 tot.Id,
                                 tot.Titulo,
                                 tot.Contenido,
                                 tot.FechaPublicacion,
                                 a.Nombre,
                             }).OrderBy(result => result.FechaPublicacion).ToList();
            if (listaPost.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listaPost);
        }
    }
}
