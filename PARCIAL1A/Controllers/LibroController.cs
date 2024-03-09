using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL1A.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PARCIAL1A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ParcialContext _ParcialContexto;

        public LibroController(ParcialContext ParcialContexto)
        {
            _ParcialContexto = ParcialContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<libro> listaLibros = _ParcialContexto.Libros.ToList();
            if (listaLibros.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listaLibros);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            libro? libro = (from e in _ParcialContexto.Libros
                            where e.Id == id
                            select e).FirstOrDefault();
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] libro libro)
        {
            try
            {
                _ParcialContexto.Libros.Add(libro);
                _ParcialContexto.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(int id, [FromBody] libro libroModificar)
        {
            libro? libroActual = (from e in _ParcialContexto.Libros
                                  where e.Id == id
                                  select e).FirstOrDefault();
            if (libroActual == null)
            {
                return NotFound();
            }

            libroActual.Titulo = libroModificar.Titulo;

            _ParcialContexto.Entry(libroActual).State = EntityState.Modified;
            _ParcialContexto.SaveChanges();

            return Ok(libroModificar);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            libro? libro = (from e in _ParcialContexto.Libros
                            where e.Id == id
                            select e).FirstOrDefault();
            if (libro == null)
            {
                return NotFound();
            }

            _ParcialContexto.Libros.Remove(libro);
            _ParcialContexto.SaveChanges();

            return Ok(libro);


        }
        [HttpGet]
        [Route("BuscarPorAutor/{nombreAutor}")]
        public IActionResult BuscarPorAutor(string nombreAutor)
        {
            List<libro> librosDelAutor = _ParcialContexto.Libros
                .Join(_ParcialContexto.AutorLibro,
                      libro => libro.Id,
                      autorLibro => autorLibro.LibroId,
                      (libro, autorLibro) => new { Libro = libro, AutorLibro = autorLibro })
                .Join(_ParcialContexto.Autores,
                      combined => combined.AutorLibro.AutorId,
                      autor => autor.Id,
                      (combined, autor) => new { combined.Libro, Autor = autor })
                .Where(result => result.Autor.Nombre == nombreAutor)
                .Select(result => result.Libro)
                .ToList();

            if (librosDelAutor.Count() == 0)
            {
                return NotFound("No se encontraron libros para el autor especificado.");
            }

            return Ok(librosDelAutor);
        }
    }
}

  