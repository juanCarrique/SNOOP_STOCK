using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using Domain.Models;
using Services.Services;
using Services;
using DataAccess;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly FacturaService _facturaService;

        public FacturasController(FacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            var facturas = await _facturaService.GetFacturas();
            return Ok(facturas);
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _facturaService.GetFactura(id);

            if (factura == null)
                return NotFound("Factura no encontrada.");

            return Ok(factura);
        }

        // GET: api/Facturas/5/Items
        [HttpGet("{id}/Items")]
        public async Task<ActionResult<ICollection<ItemFacturaDTO>>> GetFacturaItems(int id)
        {
            var facturaItems = await _facturaService.GetFacturaItems(id);

            if (facturaItems == null)
                return NotFound("Factura no encontrada.");

            return Ok(facturaItems);
        }

        // PUT: api/Facturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<FacturaDTO>> PutFactura(int id, FacturaDTO facturaDTO)
        {
            if (id != facturaDTO.Id)
                return BadRequest("Los IDs no coinciden.");

            try
            {
                var resultado = await _facturaService.PutFactura(facturaDTO);

                if (resultado == null)
                    return NotFound("Factura no encontrada.");


                return resultado;

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Facturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostFactura(FacturaDTO facturaDTO)
        {

            if(facturaDTO == null)
                return BadRequest("Factura no encontrada.");

            try
            {
                var facturaId = await _facturaService.PostFactura(facturaDTO);

                if(facturaId == 0)
                    return BadRequest("Factura no encontrada.");

                return CreatedAtAction(nameof(GetFactura), new { id = facturaId }, facturaId);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<FacturaDTO>> PatchFactura(int id, FacturaUpdateDTO facturaUpdateDTO)
        {
            if (facturaUpdateDTO == null)
                return BadRequest("Datos de la factura no proporcionados.");

            try
            {
                var resultado = await _facturaService.PatchFactura(id, facturaUpdateDTO);

                if (resultado == null)
                    return NotFound("Factura no encontrada.");

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var resultado = await _facturaService.DeleteFactura(id);

            if (resultado == null)
                return NotFound("Factura no encontrada.");

            return Ok(resultado);
        }

        private async Task<bool> FacturaExists(int id)
        {
            return await _facturaService.FacturaExists(id);
    }
    }
}
