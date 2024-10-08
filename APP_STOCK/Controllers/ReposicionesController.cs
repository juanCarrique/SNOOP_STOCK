﻿using System;
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

        // GET: api/Reposiciones/5/Producto
        [HttpGet("{id}/Producto")]
        public async Task<ActionResult<Producto>> GetProductoByReposicionId(int id)
        {
            if(!await ReposicionExists(id))
            {
                return NotFound("Reposicion no encontrada.");
            }


            var producto = await _reposicionService.GetProductoByReposicionId(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // GET: api/Reposiciones/5/Proveedor
        [HttpGet("{id}/Proveedor")]
        public async Task<ActionResult<Proveedor>> GetProveedorByReposicionId(int id)
        {
            if (!await ReposicionExists(id))
            {
                return NotFound("Reposicion no encontrada.");
            }

            var proveedor = await _reposicionService.GetProveedorByReposicionId(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return Ok(proveedor);
        }

        // PUT: api/Reposiciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ReposicionDTO>> PutReposicion(int id, ReposicionDTO reposicionDTO)
        {
            if (id != reposicionDTO.Id)
                return BadRequest("Id no coincide.");

            try
            {
                var resultado = await _reposicionService.PutReposicion(reposicionDTO);

                if (resultado == null)
                    return NotFound("Reposicion no encontrada.");

                return Ok(resultado);

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
        public async Task<ActionResult<int>> PostReposicion(ReposicionDTO reposicionDTO)
        {
            if (reposicionDTO == null)
                return BadRequest("Datos de la reposicion no proporcionados.");

            try
            {
                var reposicionId = await _reposicionService.PostReposicion(reposicionDTO);

                if (reposicionId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la reposicion.");

                return CreatedAtAction(nameof(GetReposicion), new { id = reposicionId }, reposicionId);
            }
            catch (ArgumentException ex)
            {
                // Retorna 400 Bad Request con el mensaje de error
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Reposiciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReposicionDTO>> DeleteReposicion(int id)
        {
            var resultado = await _reposicionService.DeleteReposicion(id);

            if (resultado == null)
                return NotFound("Reposicion no encontrada.");

            return resultado;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ReposicionDTO>> PatchReposicion(int id, ReposicionUpdateDTO reposicionUpdateDTO)
        {
            if (reposicionUpdateDTO == null)
                return BadRequest("Datos la reposicion no proporcionados.");

            try
            {
                
                var resultado = await _reposicionService.PatchReposicion(id, reposicionUpdateDTO);

                if (resultado == null)
                    return NotFound("Reposicion no encontrada.");

                return Ok(resultado);
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