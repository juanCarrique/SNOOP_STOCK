using DataAccess;
using DataAccess.DAOs;
using Domain.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UsuarioService
    {
        private readonly UsuarioDAO _usuarioDAO;

        public UsuarioService(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetUsuarios()
        {
            var usuarios = await _usuarioDAO.GetUsuarios();
            var usuariosDTO = usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Mail = u.Mail,
                Telefono = u.Telefono,
                Password = u.Password,
                RolId = u.RolId,
                Estado = u.Estado
            }).ToList();

            return usuariosDTO;
        }

        public async Task<UsuarioDTO> GetUsuario(int id)
        {
            var usuario = await _usuarioDAO.GetUsuario(id);

            if (usuario == null)
            {
                return null;
            }

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Mail = usuario.Mail,
                Telefono = usuario.Telefono,
                Password = usuario.Password,
                RolId = usuario.RolId,
                Estado = usuario.Estado
            };
        }

        public async Task<bool> UsuarioExists(int id)
        {
            return await _usuarioDAO.UsuarioExists(id);
        }

        public async Task<bool> PutUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await _usuarioDAO.GetUsuario(usuarioDTO.Id);

                if (usuario == null)
                {
                    return false;
                }

                usuario.Nombre = usuarioDTO.Nombre;
                usuario.Apellido = usuarioDTO.Apellido;
                usuario.Mail = usuarioDTO.Mail;
                usuario.Telefono = usuarioDTO.Telefono;
                usuario.Password = usuarioDTO.Password;
                usuario.RolId = usuarioDTO.RolId;
                usuario.Estado = usuarioDTO.Estado;

                _usuarioDAO.UpdateUsuario(usuario);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int> PostUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Mail = usuarioDTO.Mail,
                Telefono = usuarioDTO.Telefono,
                Password = usuarioDTO.Password,
                RolId = usuarioDTO.RolId,
                Estado = usuarioDTO.Estado
            };

            _usuarioDAO.AddUsuario(usuario);

            await _usuarioDAO.SaveChangesAsync();

            return usuario.Id;
        }

        public async Task<bool> DeleteUsuario(int id)
        {
            var usuario = await _usuarioDAO.GetUsuario(id);

            if (usuario == null)
            {
                return false;
            }

            _usuarioDAO.DeleteUsuario(usuario);

            await _usuarioDAO.SaveChangesAsync();

            return true;
        }
    }
}
