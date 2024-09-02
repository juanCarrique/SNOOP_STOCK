using DataAccess;
using DataAccess.DAOs;
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
    public class HistorialMovimientoService
    {
        private readonly HistorialMovimientoDAO _historialMovimientoDAO;
        public HistorialMovimientoService(HistorialMovimientoDAO historialMovimientoDAO)
        {
            _historialMovimientoDAO = historialMovimientoDAO;
        }
        public async Task<IEnumerable<HistorialMovimientoDTO>> GetHistorialMovimientos()
        {
            var historialMovimientos = await _historialMovimientoDAO.GetHistorialMovimientos();
            var historialMovimientoDTO = historialMovimientos.Select(hm => new HistorialMovimientoDTO
            {
                Id = hm.Id,
                ProductoId = hm.ProductoId,
                TipoId = hm.TipoId,
                Cantidad = hm.Cantidad,
                Fecha = hm.Fecha,
                UsuarioId = hm.UsuarioId,
                Detalle = hm.Detalle
            }).ToList();
            return historialMovimientoDTO;
        }
        public async Task<HistorialMovimientoDTO> GetHistorialMovimiento(int id)
        {
            var historialMovimiento = await _historialMovimientoDAO.GetHistorialMovimiento(id);
            if (historialMovimiento == null)
            {
                return null;
            }
            return new HistorialMovimientoDTO
            {
                Id = historialMovimiento.Id,
                ProductoId = historialMovimiento.ProductoId,
                TipoId = historialMovimiento.TipoId,
                Cantidad = historialMovimiento.Cantidad,
                Fecha = historialMovimiento.Fecha,
                UsuarioId = historialMovimiento.UsuarioId,
                Detalle = historialMovimiento.Detalle
            };
        }
        public async Task<bool> HistorialMovimientoExists(int id)
        {
            return await _historialMovimientoDAO.HistorialMovimientoExists(id);
        }
        public async Task<HistorialMovimientoDTO> PutHistorialMovimiento(HistorialMovimientoDTO historialMovimientoDTO)
        {
            try
            {
                var historialMovimiento = await _historialMovimientoDAO.GetHistorialMovimiento(historialMovimientoDTO.Id);
                if (historialMovimiento == null)
                {
                    return null;
                }
                historialMovimiento.UsuarioId = historialMovimientoDTO.UsuarioId;
                historialMovimiento.ProductoId = historialMovimientoDTO.ProductoId;
                historialMovimiento.TipoId = historialMovimientoDTO.TipoId;
                historialMovimiento.Cantidad = historialMovimientoDTO.Cantidad;
                historialMovimiento.Fecha = historialMovimientoDTO.Fecha;
                historialMovimiento.Detalle = historialMovimientoDTO.Detalle;     
                _historialMovimientoDAO.UpdateHistorialMovimiento(historialMovimiento);
                await _historialMovimientoDAO.SaveChangesAsync();
                return new HistorialMovimientoDTO
                {
                    Id = historialMovimiento.Id,
                    UsuarioId = historialMovimiento.UsuarioId,
                    ProductoId = historialMovimiento.ProductoId,
                    TipoId = historialMovimiento.TipoId,
                    Cantidad = historialMovimiento.Cantidad,
                    Fecha = historialMovimiento.Fecha,
                    Detalle = historialMovimiento.Detalle
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public async Task<int> PostHistorialMovimiento(HistorialMovimientoDTO historialMovimientoDTO )
        {
            var historialMovimiento = new Domain.Models.HistorialMovimiento
            {
                UsuarioId = historialMovimientoDTO.UsuarioId,
                ProductoId = historialMovimientoDTO.ProductoId,
                TipoId = historialMovimientoDTO.TipoId,
                Cantidad = historialMovimientoDTO.Cantidad,
                Fecha = historialMovimientoDTO.Fecha,
                Detalle = historialMovimientoDTO.Detalle
            };
            _historialMovimientoDAO.AddHistorialMovimiento(historialMovimiento);
            await _historialMovimientoDAO.SaveChangesAsync();
            return historialMovimiento.Id;
        }
        public async Task<HistorialMovimientoDTO> DeleteHistorialMovimiento(int id)
        {
            var historialMovimiento = await _historialMovimientoDAO.GetHistorialMovimiento(id);
            if (historialMovimiento == null)
            {
                return null;
            }
            var historialMovimientoDTO = new HistorialMovimientoDTO
            {
                Id = historialMovimiento.Id,
                UsuarioId = historialMovimiento.UsuarioId,
                ProductoId = historialMovimiento.ProductoId,
                TipoId = historialMovimiento.TipoId,
                Cantidad = historialMovimiento.Cantidad,
                Fecha = historialMovimiento.Fecha,
                Detalle = historialMovimiento.Detalle
            };
            _historialMovimientoDAO.DeleteHistorialMovimiento(historialMovimiento);
            await _historialMovimientoDAO.SaveChangesAsync();
            return historialMovimientoDTO;
        }
    }
}
