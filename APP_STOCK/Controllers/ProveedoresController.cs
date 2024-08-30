using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Services.Services;
using Services;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ProveedoresService _proveedoresService;

        public ProveedoresController(ProveedoresService proveedoresService)
        {
            _proveedoresService = proveedoresService;
        }

        // GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            var proveedores = await _proveedoresService.GetProveedores();
            return Ok(proveedores);
        }

        // GET: api/Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _proveedoresService.GetProveedor(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return Ok(proveedor);
        }

        // PUT: api/Proveedores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, ProveedorDTO proveedorDTO)
        {
            if (id != proveedorDTO.Id)
            {
                return BadRequest("El ID del proveedor no coincide.");
            }

            try
            {
                var resultado = await _proveedoresService.PutProveedor(proveedorDTO);

                if (!resultado)
                {
                    return NotFound("Proveedor no encontrado.");
                }

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProveedorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

            // POST: api/Proveedores
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<Proveedor>> PostProveedor(ProveedorDTO proveedorDTO)
            {
                if (proveedorDTO == null)
                {
                    return BadRequest("Datos del proveedor no proporcionados.");
                }

                try
                {
                    var proveedorId = await _proveedoresService.PostProveedor(proveedorDTO);

                    if (proveedorId == 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el proveedor.");
                    }

                    return CreatedAtAction(nameof(GetProveedor), new { id = proveedorId }, proveedorDTO);
                }
                catch (ArgumentException ex)
                {
                    // Retorna 400 Bad Request con el mensaje de error
                    return BadRequest(ex.Message);
                }
            }

            // DELETE: api/Proveedores/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProveedor(int id)
            {
                var resultado = await _proveedoresService.DeleteProveedor(id);

                if (!resultado)
                {
                    return NotFound();
                }

                return NoContent();
            }

            [HttpPatch("{id}")]
            public async Task<IActionResult> PatchProveedor(int id, ProveedorUpdateDTO proveedorUpdateDTO)
            {
                if (proveedorUpdateDTO == null)
                {
                    return BadRequest("Datos del proveedor no proporcionados.");
                }

                try
                {
                    var resultado = await _proveedoresService.PatchProveedor(id, proveedorUpdateDTO);

                    if (!resultado)
                    {
                        return NotFound("Proveedor no encontrado.");
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
                    if (!await ProveedorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            private async Task<bool> ProveedorExists(int id)
            {
                return await _proveedoresService.ProveedorExists(id);
            }
    }
}
