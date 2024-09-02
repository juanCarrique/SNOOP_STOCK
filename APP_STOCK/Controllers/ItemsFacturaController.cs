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
            if(itemFactura == null)
                return BadRequest("ItemFactura no puede ser nulo.");

            try
            {
                var resultado = await _itemFacturaService.PostItemFactura(itemFactura);
                if (resultado == 0)
                    return NotFound("No se pudo agregar el ItemFactura.");

                return CreatedAtAction(nameof(GetItemFactura), new { id = resultado }, resultado); ;
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }

        }

        // PATCH: api/ItemsFactura/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<ItemFacturaDTO>> PatchItemFactura(int id, ItemFacturaUpdateDTO itemFacturaUpdateDTO)
        {
            if (itemFacturaUpdateDTO == null)
                return BadRequest("Datos del item no proporcionados.");

            try
            {
                var resultado = await _itemFacturaService.PatchItemFactura(id, itemFacturaUpdateDTO);

                if (resultado == null)
                    return NotFound("ItemFactura no encontrado.");

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
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


        // DELETE: api/ItemsFactura/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemFacturaDTO>> DeleteItemFactura(int id)
        {
            var resultado = await _itemFacturaService.DeleteItemFactura(id);
            
            if (resultado == null)
                return NotFound("No se encontro el ItemFactura");

            return resultado;
        }

        private async Task<bool> ItemFacturaExists(int id)
        {
            return await _itemFacturaService.ItemFacturaExists(id);
        }
    }
}
