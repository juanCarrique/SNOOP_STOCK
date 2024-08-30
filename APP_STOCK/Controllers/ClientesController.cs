using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using Services;
using DataAccess;

namespace AppStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            var clientes = await _clienteService.GetClientes();
            return Ok(clientes);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            var cliente = await _clienteService.GetCliente(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteDTO>> PutCliente(int id, ClienteDTO clienteDTO)
        {
            if (id != clienteDTO.Id)
                return BadRequest("El Id del cliente no coincide.");

            try
            {
                var resultado = await _clienteService.PutCliente(clienteDTO);

                if (resultado == null)
                    return NotFound("Cliente no encontrado.");

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostCliente(ClienteDTO clienteDTO)
        {
            if (clienteDTO == null)
                return BadRequest("Datos del cliente no proporcionados.");

            try
            {

                var resultado = await _clienteService.PostCliente(clienteDTO);

                if (resultado == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el producto.");

                return CreatedAtAction("GetCliente", new { id = resultado }, resultado);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteDTO>> DeleteCliente(int id)
        {

            var resultado = await _clienteService.DeleteCliente(id);

            if (resultado == null)
                return NotFound("Cliente no encontrado.");
            
            return Ok(resultado);

        }


        // PATCH: api/Clientes/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<ClienteDTO>> PatchCliente(int id, ClienteUpdateDTO clienteUpdateDTO)
        {
            if(clienteUpdateDTO == null)
                return BadRequest("Datos del cliente no proporcionados.");

            try
            {
                var resultado = await _clienteService.PatchCliente(id, clienteUpdateDTO);

                if (resultado == null)
                    return NotFound("Cliente no encontrado.");

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private Task<bool> ClienteExists(int id)
        {
            return _clienteService.ClienteExists(id);
        }
    }
}
