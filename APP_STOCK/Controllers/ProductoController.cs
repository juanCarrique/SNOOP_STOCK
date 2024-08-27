using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Domain.Models;

namespace APP_STOCK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _productoService.GetProductos();
            return Ok(productos);
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productoService.GetProducto(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        //PUT: api/Productoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, ProductoDTO productoDTO)
        {
            if (id != productoDTO.Id)
            {
                return BadRequest("El ID del producto no coincide.");
            }


            try
            {
                var resultado = await _productoService.PutProducto(productoDTO);

                if (!resultado)
                {
                    return NotFound("Producto no encontrado.");
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
                if (! await ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Productoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(ProductoDTO productoDTO)
        {

            if (productoDTO == null)
            {
                return BadRequest("Datos del producto no proporcionados.");
            }

            try
            {
                var productoId = await _productoService.PostProducto(productoDTO);

                if (productoId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el producto.");
                }

                return CreatedAtAction(nameof(GetProducto), new { id = productoId }, productoDTO);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var resultado = await _productoService.DeleteProducto(id);

            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProducto(int id, ProductoUpdateDTO productoUpdateDTO)
        {
            if (productoUpdateDTO == null)
            {
                return BadRequest("Datos del producto no proporcionados.");
            }

            try
            {
                var resultado = await _productoService.PatchProducto(id, productoUpdateDTO);

                if (!resultado)
                {
                    return NotFound("Producto no encontrado.");
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
                if (!await ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<bool> ProductoExists(int id)
        {
            return await _productoService.ProductoExists(id);
        }
    }
}
