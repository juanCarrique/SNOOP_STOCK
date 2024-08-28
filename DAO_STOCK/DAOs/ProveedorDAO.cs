using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ProveedorDAO
    {
        private readonly StockappContext _context;

        public ProveedorDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetProveedores()
        {
            return await _context.Proveedores.ToListAsync();
        }

        public async Task<Proveedor> GetProveedor(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        public void AddProveedor(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
        }

        public void UpdateProveedor(Proveedor proveedor)
        {
            _context.Entry(proveedor).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProveedorExists(int id)
        {
            return await _context.Proveedores.AnyAsync(p => p.Id == id);
        }

        public void DeleteProveedor(Proveedor proveedor)
        {
            _context.Proveedores.Remove(proveedor);
        }

    }
}
