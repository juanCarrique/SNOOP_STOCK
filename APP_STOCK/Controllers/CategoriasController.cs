using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            var categorias = await _categoriaService.GetCategorias();
            return Ok(categorias);
        }

        // GET: api/Categorias/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categorias = await _categoriaService.GetCategoria(id);

            if (categorias == null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        //PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.Id)
            {
                return BadRequest("El ID de la categoria no coincide.");
            }


            try
            {
                var resultado = await _categoriaService.PutCategoria(categoriaDTO);

                if (!resultado)
                {
                    return NotFound("Categoria no encontrada.");
                }

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(CategoriaDTO categoriaDTO)
        {

            if (categoriaDTO == null)
            {
                return BadRequest("Datos de la categoria no proporcionados.");
            }

            try
            {
                var categoriaId = await _categoriaService.PostCategoria(categoriaDTO);

                if (categoriaId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la categoria.");
                }

                return CreatedAtAction(nameof(GetCategoria), new { id = categoriaId }, categoriaDTO);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var resultado = await _categoriaService.DeleteCategoria(id);

            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<bool> CategoriaExists(int id)
        {
            return await _categoriaService.CategoriaExists(id);
        }

    }
}
