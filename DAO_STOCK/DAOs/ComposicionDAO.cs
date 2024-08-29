using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ComposicionDAO
    {
        private readonly StockappContext _context;

        public ComposicionDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Composicion>> GetComposiciones()
        {
            return await _context.Composiciones
                .Include(c => c.Producto)
                .Include(c => c.Componente)
                .ToListAsync();
        }

        public async Task<Composicion> GetComposicion(int id)
        {
            return await _context.Composiciones
                .Include(c => c.Producto)
                .Include(c => c.Componente)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Composicion>> GetComposicionesByProductoId(int id)
        {
            return await _context.Composiciones
                .Include(c => c.Producto)
                .Include(c => c.Componente)
                .Where(c => c.ProductoId == id)
                .ToListAsync();
        }

        public void AddComposicion(Composicion composicion)
        {
            _context.Composiciones.Add(composicion);
        }

        public void UpdateComposicion(Composicion composicion)
        {
            _context.Entry(composicion).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ComposicionExists(int id)
        {
            return await _context.Composiciones.AnyAsync(p => p.Id == id);
        }

        public void DeleteComposicion(Composicion composicion)
        {
            _context.Composiciones.Remove(composicion);
        }
    }
}
