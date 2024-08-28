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
    public class ReposicionesController : ControllerBase
    {
        private readonly ReposicionService _reposicionService;

        public ReposicionesController(ReposicionService reposicionService)
        {
            _reposicionService = reposicionService;
        }

        // GET: api/Reposiciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reposicion>>> GetReposiciones()
        {
            var reposiciones = await _reposicionService.GetReposiciones();
            return Ok(reposiciones);
        }

        // GET: api/Reposiciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reposicion>> GetReposicion(int id)
        {
            var reposicion = await _reposicionService.GetReposicion(id);

            if (reposicion == null)
            {
                return NotFound();
            }

            return Ok(reposicion);
        }

        // PUT: api/Reposiciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReposicion(int id, ReposicionDTO reposicionDTO)
        {
            if (id != reposicionDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                var resultado = await _reposicionService.PutReposicion(reposicionDTO);

                if (!resultado)
                {
                    return NotFound("Reposicion no encontrada.");
                }

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReposicionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Reposiciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reposicion>> PostReposicion(ReposicionDTO reposicionDTO)
        {
            if (reposicionDTO == null)
            {
                return BadRequest("Datos de la reposicion no proporcionados.");
            }

            try
            {
                var reposicionId = await _reposicionService.PostReposicion(reposicionDTO);

                if (reposicionId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la reposicion.");
                }

                return CreatedAtAction(nameof(GetReposicion), new { id = reposicionId }, reposicionDTO);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Reposiciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReposicion(int id)
        {
            var resultado = await _reposicionService.DeleteReposicion(id);

            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchReposicion(int id, ReposicionUpdateDTO reposicionUpdateDTO)
        {
            if (reposicionUpdateDTO == null)
            {
                return BadRequest("Datos la reposicion no proporcionados.");
            }

            try
            {
                
                var resultado = await _reposicionService.PatchReposicion(id, reposicionUpdateDTO);

                if (!resultado)
                {
                    return NotFound("Reposicion no encontrada.");
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReposicionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }
        private async Task<bool> ReposicionExists(int id)
        {
            return await _reposicionService.ReposicionExists(id);
        }
    }
}