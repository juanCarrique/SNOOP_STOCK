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
using Services;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialMovimientosController : ControllerBase
    {
        private readonly HistorialMovimientoService _historialMovimientoService;
        public HistorialMovimientosController(HistorialMovimientoService historialMovimientoService)
        {
            _historialMovimientoService = historialMovimientoService;
        }

        // GET: api/HistorialMovimientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialMovimientoDTO>>> GetHistorialMovimientos()
        {
            var historialMovimientos = await _historialMovimientoService.GetHistorialMovimientos();
            return Ok(historialMovimientos);
        }

        // GET: api/HistorialMovimientos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialMovimientoDTO>> GetHistorialMovimiento(int id)
        {
            var historialMovimiento = await _historialMovimientoService.GetHistorialMovimiento(id);

            if (historialMovimiento == null)
            {
                return NotFound();
            }

            return Ok(historialMovimiento);
        }

        // PUT: api/HistorialMovimientos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<HistorialMovimientoDTO>> PutHistorialMovimiento(int id, HistorialMovimientoDTO historialMovimientoDTO)
        {
            if (id != historialMovimientoDTO.Id)
                return BadRequest("El ID del historial de movimiento no coincide.");

            try
            {
                var resultado = await _historialMovimientoService.PutHistorialMovimiento(historialMovimientoDTO);

                if (resultado == null)
                    return NotFound("Historial de movimiento no encontrado.");

                return Ok(resultado);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/HistorialMovimientos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostHistorialMovimiento(HistorialMovimientoDTO historialMovimientoDTO)
        {

            if (historialMovimientoDTO == null)
            {
                return BadRequest("Datos de la categoria no proporcionados.");
            }

            try
            {
                var historialMovimientoId = await _historialMovimientoService.PostHistorialMovimiento(historialMovimientoDTO);

                if (historialMovimientoId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el historial de Movimientos.");

                return CreatedAtAction(nameof(GetHistorialMovimiento), new { id = historialMovimientoId }, historialMovimientoId);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/HistorialMovimientos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HistorialMovimientoDTO>> DeleteHistorialMovimiento(int id)
        {
            var resultado = await _historialMovimientoService.DeleteHistorialMovimiento(id);

            if (resultado == null)
                return NotFound("Historial de movimiento no encontrado.");

            return Ok(resultado);
        }

        private async Task<bool> HistorialMovimientoExists(int id)
        {
            return await _historialMovimientoService.HistorialMovimientoExists(id);
        }
    }
}
