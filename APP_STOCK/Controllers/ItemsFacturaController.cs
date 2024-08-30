using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using Domain.Models;
using Services;
using DataAccess;
using Services.Services;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsFacturaController : ControllerBase
    {
        private readonly ItemFacturaService _itemFacturaService;

        public ItemsFacturaController(ItemFacturaService itemFacturaService)
        {
            _itemFacturaService = itemFacturaService;
        }

        // GET: api/ItemsFactura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemFacturaDTO>>> GetItemsFactura()
        {
            var itemsFactura = await _itemFacturaService.GetItemsFactura();
            return Ok(itemsFactura);
        }

        // GET: api/ItemsFactura/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemFacturaDTO>> GetItemFactura(int id)
        {
            var itemFactura = await _itemFacturaService.GetItemFactura(id);

            if (itemFactura == null)
                return NotFound("No se encontro el ItemFactura");

            return Ok(itemFactura);
        }

        // PUT: api/ItemsFactura/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemFacturaDTO>> PutItemFactura(int id, ItemFacturaDTO itemFactura)
        {
            if (id != itemFactura.Id)
                return BadRequest("Los IDs no coinciden.");
            try
            {
                var resultado = await _itemFacturaService.PutItemFactura(itemFactura);

                if (resultado == null)
                    return NotFound("itemFactura no encontrada.");


                return resultado;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemFacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/ItemsFactura
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemFacturaDTO>> PostItemFactura(ItemFacturaDTO itemFactura)
        {
            _context.ItemsFactura.Add(itemFactura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemFactura", new { id = itemFactura.Id }, itemFactura);
        }

        // DELETE: api/ItemsFactura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemFactura(int id)
        {
            var itemFactura = await _context.ItemsFactura.FindAsync(id);
            if (itemFactura == null)
            {
                return NotFound();
            }

            _context.ItemsFactura.Remove(itemFactura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ItemFacturaExists(int id)
        {
            return await _itemFacturaService.ItemFacturaExists(id);
        }
    }
}
