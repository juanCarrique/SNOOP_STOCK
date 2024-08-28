
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using Domain.Models;
using Services.Services;
using Services;
using Services;
using DataAccess;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComposicionesController : ControllerBase
    {
        private readonly ComposicionService _composicionService;

        public ComposicionesController(ComposicionService composicionService)
        {
            _composicionService = composicionService;
        }

        // GET: api/Composiciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Composicion>>> GetComposiciones()
        {
            var composiciones = await _composicionService.GetComposiciones();
            return Ok(composiciones);
        }

        // GET: api/Composicions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Composicion>> GetComposicion(int id)
        {
            var composicion = await _composicionService.GetComposicion(id);

            if (composicion == null)
            {
                return NotFound();
            }

            return Ok(composicion);
        }

        // PUT: api/Composicions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComposicion(int id, ComposicionDTO composicionDTO)
        {
            if (id != composicionDTO.Id)
            {
                return BadRequest("El ID no coincide con la composicion");
            }

            try
            {
                var resultado = await _composicionService.PutComposicion(composicionDTO);

                if (!resultado)
                {
                    return NotFound("Composicion no encontrada.");
                }

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ComposicionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Composicions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Composicion>> PostComposicion(ComposicionDTO composicionDTO)
        {
            if (composicionDTO == null)
            {
                return BadRequest("Datos de la composicion no proporcionados.");
            }

            try
            {
                var composicionId = await _composicionService.PostComposicion(composicionDTO);

                if (composicionId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la composicion.");
                }

                return CreatedAtAction(nameof(GetComposicion), new { id = composicionId }, composicionDTO);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Composicions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComposicion(int id)
        {
            var resultado = await _composicionService.DeleteComposicion(id);

            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<bool> ComposicionExists(int id)
        {
            return await _composicionService.ComposicionExists(id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProducto(int id, ComposicionUpdateDTO composicionUpdateDTO)
        {
            if (composicionUpdateDTO == null)
            {
                return BadRequest("Datos de la composicion no proporcionados.");
            }

            try
            {
                var resultado = await _composicionService.PatchComposicion(id, composicionUpdateDTO);

                if (!resultado)
                {
                    return NotFound("Composicion no encontrada.");
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ComposicionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
