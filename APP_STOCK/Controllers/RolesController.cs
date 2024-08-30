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
using Humanizer;
using Services.DTOs;
using Services;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolesController(RolService rolService)
        {
            _rolService = rolService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetRoles()
        {
            var roles = await _rolService.GetRoles();
            return Ok(roles);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolDTO>> GetRol(int id)
        {
            var rol = await _rolService.GetRol(id);

            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rol);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, RolDTO rolDTO)
        {
            if (id != rolDTO.Id)
            {
                return BadRequest("El ID del rol no coincide.");
            }

            try
            {
                var resultado = await _rolService.PutRol(rolDTO);

                if (!resultado)
                {
                    return NotFound("Categoria no encontrada.");
                }

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(RolDTO rolDTO)
        {

            if (rolDTO == null)
            {
                return BadRequest("Datos de la categoria no proporcionados.");
            }

            try
            {
                var RolId = await _rolService.PostRol(rolDTO);

                if (RolId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la categoria.");
                }

                return CreatedAtAction(nameof(GetRol), new { id = RolId }, rolDTO);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var resultado = await _rolService.DeleteRol(id);

            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<bool> RolExists(int id)
        {
            return await _rolService.RolExists(id);
        }
    }
}
