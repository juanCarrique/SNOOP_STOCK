using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class FacturaDAO
    {
        private readonly StockappContext _context;

        public FacturaDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Factura>> GetFacturas()
        {
            return await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.ItemsFactura)
                .ToListAsync();
        }

        public async Task<Factura> GetFactura(int id)
        {
            return await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.ItemsFactura)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public void AddFactura(Factura factura)
        {
            _context.Facturas.Add(factura);
        }

        public void UpdateFactura(Factura factura)
        {
            _context.Entry(factura).State = EntityState.Modified;
        }
        public void DeleteFactura(Factura factura)
        {
            _context.Facturas.Remove(factura);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> FacturaExists(int id)
        {
            return await _context.Facturas.AnyAsync(f => f.Id == id);
        }

    }
}
