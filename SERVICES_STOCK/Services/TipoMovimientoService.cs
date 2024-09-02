using DataAccess.DAOs;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TipoMovimientoService
    {
        private readonly TipoMovimientoDAO _tipoMovimientoDAO;
        public TipoMovimientoService(TipoMovimientoDAO tipoMovimientoDAO)
        {
            _tipoMovimientoDAO = tipoMovimientoDAO;
        }
        public async Task<IEnumerable<TipoMovimientoDTO>> GetTipoMovimientos()
        {
            var tipoMovimientos = await _tipoMovimientoDAO.GetTipoMovimientos();
            var tipoMovimientoDTO = tipoMovimientos.Select(tm => new TipoMovimientoDTO
            {
                Id = tm.Id,
                Nombre = tm.Nombre,
                Descripcion = tm.Descripcion
            }).ToList();
            return tipoMovimientoDTO;
        }
        public async Task<TipoMovimientoDTO> GetTipoMovimiento(int id)
        {
            var tipoMovimiento = await _tipoMovimientoDAO.GetTipoMovimiento(id);
            if (tipoMovimiento == null)
            {
                return null;
            }
            return new TipoMovimientoDTO
            {
                Id = tipoMovimiento.Id,
                Nombre = tipoMovimiento.Nombre,
                Descripcion = tipoMovimiento.Descripcion
            };
        }
        public async Task<bool> TipoMovimientoExists(int id)
        {
            return await _tipoMovimientoDAO.TipoMovimientoExists(id);
        }
        public async Task<bool> PutTipoMovimiento(TipoMovimientoDTO tipoMovimientoDTO)
        {
            try
            {
                var tipoMovimiento = await _tipoMovimientoDAO.GetTipoMovimiento(tipoMovimientoDTO.Id);
                if (tipoMovimiento == null)
                {
                    return false;
                }
                tipoMovimiento.Nombre = tipoMovimientoDTO.Nombre;
                tipoMovimiento.Descripcion = tipoMovimientoDTO.Descripcion;
                _tipoMovimientoDAO.UpdateTipoMovimiento(tipoMovimiento);
                await _tipoMovimientoDAO.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int> PostTipoMovimiento(TipoMovimientoDTO tipoMovimientoDTO)
        {
            var tipoMovimiento = new Domain.Models.TipoMovimiento
            {
                Nombre = tipoMovimientoDTO.Nombre,
                Descripcion = tipoMovimientoDTO.Descripcion
            };
            _tipoMovimientoDAO.AddTipoMovimiento(tipoMovimiento);
            await _tipoMovimientoDAO.SaveChangesAsync();
            return tipoMovimiento.Id;
        }
        public async Task<bool> DeleteTipoMovimiento(int id)
        {
            var tipoMovimiento = await _tipoMovimientoDAO.GetTipoMovimiento(id);
            if (tipoMovimiento == null)
            {
                return false;
            }
            _tipoMovimientoDAO.DeleteTipoMovimiento(tipoMovimiento);
            await _tipoMovimientoDAO.SaveChangesAsync();
            return true;
        }
    }
}
