using DataAccess;
using DataAccess.DAOs; //TODO preguntarle a fran esto, sin esto no me dejaba poner RolDAO
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RolService
    {
        private readonly RolDAO _rolDAO;

        public RolService(RolDAO rolDAO)
        {
            _rolDAO = rolDAO;
        }

        public async Task<IEnumerable<RolDTO>> GetRoles()
        {
            var roles = await _rolDAO.GetRoles();
            var rolDTO = roles.Select(r => new RolDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion
            }).ToList();

            return rolDTO;
        }

        public async Task<RolDTO> GetRol(int id)
        {
            var rol = await _rolDAO.GetRol(id);

            if (rol == null)
            {
                return null;
            }

            return new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion
            };
        }

        public async Task<bool> RolExists(int id)
        {
            return await _rolDAO.RolExists(id);
        }

        public async Task<bool> PutRol(RolDTO rolDTO)
        {
            try
            {
                var rol = await _rolDAO.GetRol(rolDTO.Id);

                if (rol == null)
                {
                    return false;
                }

                rol.Nombre = rolDTO.Nombre;

                _rolDAO.UpdateRol(rol);

                await _rolDAO.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> PostRol(RolDTO rolDTO)
        {
            var rol = new Rol
            {
                Nombre = rolDTO.Nombre,
                Descripcion = rolDTO.Descripcion
            };

            _rolDAO.AddRol(rol);

            await _rolDAO.SaveChangesAsync();

            return rol.Id;
        }

        public async Task<bool> DeleteRol(int id)
        {
            var rol = await _rolDAO.GetRol(id);

            if (rol == null)
            {
                return false;
            }

            _rolDAO.DeleteRol(rol);

            await _rolDAO.SaveChangesAsync();

            return true;
        }
    }
}
