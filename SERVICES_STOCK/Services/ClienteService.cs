using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ClienteService
    {
        private readonly ClienteDAO _clienteDAO;

        public ClienteService(ClienteDAO clienteDAO)
        {
            _clienteDAO = clienteDAO;
        }

        public async Task<IEnumerable<ClienteDTO>> GetClientes()
        {
            var clientes = await _clienteDAO.GetClientes();
            return clientes.Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Mail = c.Mail,
                Telefono = c.Telefono
            });
        }

        public async Task<ClienteDTO> GetCliente(int id)
        {
            var cliente = await _clienteDAO.GetCliente(id);

            if (cliente == null)
                return null;

            return new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Mail = cliente.Mail,
                Telefono = cliente.Telefono
            };
        }

        public async Task<ClienteDTO> PutCliente(ClienteDTO clienteDTO)
        {
            try
            {
                var cliente = await _clienteDAO.GetCliente(clienteDTO.Id);

                if (cliente == null)
                {
                    return null;
                }

                cliente.Nombre = clienteDTO.Nombre;
                cliente.Apellido = clienteDTO.Apellido;
                cliente.Mail = clienteDTO.Mail;
                cliente.Telefono = clienteDTO.Telefono;


                _clienteDAO.UpdateCliente(cliente);

                await _clienteDAO.SaveChangesAsync();

                return new ClienteDTO
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Mail = cliente.Mail,
                    Telefono = cliente.Telefono
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> PostCliente(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente
            {
                Nombre = clienteDTO.Nombre,
                Apellido = clienteDTO.Apellido,
                Mail = clienteDTO.Mail,
                Telefono = clienteDTO.Telefono
            };

            _clienteDAO.AddCliente(cliente);
            await _clienteDAO.SaveChangesAsync();

            return cliente.Id;
        }

        public async Task<ClienteDTO> DeleteCliente(int id)
        {
            var cliente = await _clienteDAO.GetCliente(id);

            if (cliente == null)
            {
                return null;
            }

            var clienteDTO = new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Mail = cliente.Mail,
                Telefono = cliente.Telefono
            };

            _clienteDAO.DeleteCliente(cliente);
            await _clienteDAO.SaveChangesAsync();

            return clienteDTO;
        }

        public async Task<ClienteDTO> PatchCliente(int id, ClienteUpdateDTO clienteUpdateDTO)
        {
            var cliente = await _clienteDAO.GetCliente(id);
            if (cliente == null)
            {
                return null;
            }

            if(clienteUpdateDTO.Nombre != null)
                cliente.Nombre = clienteUpdateDTO.Nombre;

            if(clienteUpdateDTO.Apellido != null)
                cliente.Apellido = clienteUpdateDTO.Apellido;

            if (clienteUpdateDTO.Mail != null)
                cliente.Mail = clienteUpdateDTO.Mail;

            if (clienteUpdateDTO.Telefono != null)
                cliente.Telefono = clienteUpdateDTO.Telefono;

            _clienteDAO.UpdateCliente(cliente);
            await _clienteDAO.SaveChangesAsync();

            return new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Mail = cliente.Mail,
                Telefono = cliente.Telefono
            };

        }

        public async Task<bool> ClienteExists(int id)
        {
            return await _clienteDAO.ClienteExists(id);
        }


    }
}
