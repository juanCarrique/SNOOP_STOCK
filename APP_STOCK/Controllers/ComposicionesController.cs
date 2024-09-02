
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
        public async Task<ActionResult<IEnumerable<ComposicionDTO>>> GetComposiciones()
        {
            var composiciones = await _composicionService.GetComposiciones();
            return Ok(composiciones);
        }

        // GET: api/Composicions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComposicionDTO>> GetComposicion(int id)
        {
            var composicion = await _composicionService.GetComposicion(id);

            if (composicion == null)
            {
                return NotFound();
            }

            return Ok(composicion);
        }

        // GET: api/Composiciones/5/Componente
        [HttpGet("{id}/Componentes")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetComponentesByProductoId(int id)
        {
            if(!await ComposicionExists(id))
            {
                return NotFound("Composicion no encontrada.");
            }

            var componentes = await _composicionService.GetComponentesByProductoId(id);

            if (componentes == null)
            {
                return NotFound();
            }

            return Ok(componentes);
        }

        // PUT: api/Composicions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ComposicionDTO>> PutComposicion(int id, ComposicionDTO composicionDTO)
        {
            if (id != composicionDTO.Id)
                return BadRequest("El ID no coincide con la composicion");

            try
            {
                var resultado = await _composicionService.PutComposicion(composicionDTO);

                if (resultado == null)
                    return NotFound("Composicion no encontrada.");

                return Ok(resultado);

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
        public async Task<ActionResult<int>> PostComposicion(ComposicionDTO composicionDTO)
        {
            if (composicionDTO == null)
            {
                return BadRequest("Datos de la composicion no proporcionados.");
            }

            try
            {
                var composicionId = await _composicionService.PostComposicion(composicionDTO);

                if (composicionId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la composicion.");

                return CreatedAtAction(nameof(GetComposicion), new { id = composicionId }, composicionId);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Composicions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ComposicionDTO>> DeleteComposicion(int id)
        {
            var resultado = await _composicionService.DeleteComposicion(id);

            if (resultado == null)
                return NotFound("Composicion no encontrada.");

            return resultado;
        }

        private async Task<bool> ComposicionExists(int id)
        {
            return await _composicionService.ComposicionExists(id);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ComposicionDTO>> PatchProducto(int id, ComposicionUpdateDTO composicionUpdateDTO)
        {
            if (composicionUpdateDTO == null)
            {
                return BadRequest("Datos de la composicion no proporcionados.");
            }

            try
            {
                var resultado = await _composicionService.PatchComposicion(id, composicionUpdateDTO);

                if (resultado == null)
                    return NotFound("Composicion no encontrada.");

                return resultado;
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
    }
}
