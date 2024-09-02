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
using Services.DTOs;
using DataAccess.DAOs;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMovimientosController : ControllerBase
    {
        private readonly TipoMovimientoService _tipoMovimientoService;

        public TipoMovimientosController(TipoMovimientoService tipoMovimiento)
        {
            _tipoMovimientoService = tipoMovimiento;
        }

        // GET: api/TipoMovimientoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoMovimientoDTO>>> GetTipoMovimientos()
        {
            var tipoMovimientos = await _tipoMovimientoService.GetTipoMovimientos();
            return Ok(tipoMovimientos);
        }

        // GET: api/TipoMovimientoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoMovimientoDTO>> GetTipoMovimiento(int id)
        {
            var tipoMovimiento = await _tipoMovimientoService.GetTipoMovimiento(id);

            if (tipoMovimiento == null)
            {
                return NotFound();
            }

            return Ok(tipoMovimiento);
        }
        // PUT: api/TipoMovimientoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoMovimiento(int id, TipoMovimientoDTO tipoMovimientoDTO)
        {
            if (id != tipoMovimientoDTO.Id)
            {
                return BadRequest("El ID del tipo de movimiento no coincide.");
            }

            try
            {
                var result = await _tipoMovimientoService.PutTipoMovimiento(tipoMovimientoDTO);
                if (result == null)
                {
                    return NotFound("Tipo de movimiento no encontrado.");
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TipoMovimientoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        // POST: api/TipoMovimientos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoMovimientoDTO>> PostTipoMovimiento(TipoMovimientoDTO tipoMovimientoDTO)
        {

            if (tipoMovimientoDTO == null)
            {
                return BadRequest("Datos de tipom de movimiento no proporcionados.");
            }

            try
            {
                var TipoMovimientoId = await _tipoMovimientoService.PostTipoMovimiento(tipoMovimientoDTO);

                if (TipoMovimientoId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el tipo de movimiento.");
                }

                return CreatedAtAction(nameof(GetTipoMovimiento), new { id = TipoMovimientoId }, tipoMovimientoDTO);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }
        // DELETE: api/TipoMovimientoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoMovimiento(int id)
        {
            var resultado = await _tipoMovimientoService.DeleteTipoMovimiento(id);

            if (resultado == null)
            {
                return NotFound("Tipo de movimiento no encontrado.");
            }

            return NoContent();
        }

        private async Task<bool> TipoMovimientoExists(int id)
        {
            return await _tipoMovimientoService.TipoMovimientoExists(id);
        }
    }
}