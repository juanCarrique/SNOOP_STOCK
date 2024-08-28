

using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class ProveedoresService
    {
        private readonly ProveedorDAO _proveedoresDAO;

        public ProveedoresService(ProveedorDAO proveedoresDAO)
        {
            _proveedoresDAO = proveedoresDAO;
        }

        public async Task<IEnumerable<ProveedorDTO>> GetProveedores()
        {
            var proveedores = await _proveedoresDAO.GetProveedores();
            var proveedoresDTO = proveedores.Select(p => new ProveedorDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Telefono = p.Telefono,
                Mail= p.Mail
            }).ToList();

            return proveedoresDTO;
        }

        public async Task<ProveedorDTO> GetProveedor(int id)
        {
            var proveedor = await _proveedoresDAO.GetProveedor(id);

            if (proveedor == null)
            {
                return null;
            }

            return new ProveedorDTO
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                Mail = proveedor.Mail
            };
        }

        public async Task<bool> PutProveedor(ProveedorDTO proveedorDTO)
        {
            try
            {
                var proveedor = await _proveedoresDAO.GetProveedor(proveedorDTO.Id);

                if (proveedor == null)
                {
                    return false;
                }

                proveedor.Nombre = proveedorDTO.Nombre;
                proveedor.Telefono = proveedorDTO.Telefono;
                proveedor.Mail = proveedorDTO.Mail;

                _proveedoresDAO.UpdateProveedor(proveedor);
                await _proveedoresDAO.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<int> PostProveedor(ProveedorDTO proveedorDTO)
        {
            var proveedor = new Proveedor
            {
                Nombre = proveedorDTO.Nombre,
                Telefono = proveedorDTO.Telefono,
                Mail = proveedorDTO.Mail
            };

            _proveedoresDAO.AddProveedor(proveedor);
            await _proveedoresDAO.SaveChangesAsync();

            return proveedor.Id;
        }

        public async Task<bool> DeleteProveedor(int id)
        {
            var proveedor = await _proveedoresDAO.GetProveedor(id);

            if (proveedor == null)
            {
                return false;
            }

            _proveedoresDAO.DeleteProveedor(proveedor);
            await _proveedoresDAO.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PatchProveedor(int id, ProveedorUpdateDTO proveedorUpdateDTO)
        {
            try
            {
                var proveedor = await _proveedoresDAO.GetProveedor(id);

                if (proveedor == null)
                {
                    return false;
                }

                if (proveedorUpdateDTO.Nombre != null)
                    proveedor.Nombre = proveedorUpdateDTO.Nombre;

                if (proveedorUpdateDTO.Telefono != null)
                    proveedor.Telefono = proveedorUpdateDTO.Telefono;

                if (proveedorUpdateDTO.Mail != null)
                    proveedor.Mail = proveedorUpdateDTO.Mail;

                _proveedoresDAO.UpdateProveedor(proveedor);
                await _proveedoresDAO.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> ProveedorExists(int id)
        {
            return await _proveedoresDAO.ProveedorExists(id);
        }

    }
}
