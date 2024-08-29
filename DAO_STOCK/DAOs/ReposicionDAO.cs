using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ReposicionDAO
    {
        private readonly StockappContext _context;

        public ReposicionDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reposicion>> GetReposiciones()
        {
            return await _context.Reposiciones
                .Include(r => r.Producto)
                .Include(r => r.Proveedor)
                .ToListAsync();
        }

        public async Task<Reposicion> GetReposicion(int id)
        {
            return await _context.Reposiciones
                .Include(r => r.Producto)
                .Include(r => r.Proveedor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public void AddReposicion(Reposicion reposicion)
        {
            _context.Reposiciones.Add(reposicion);
        }

        public void UpdateReposicion(Reposicion reposicion)
        {
            _context.Entry(reposicion).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ReposicionExists(int id)
        {
            return await _context.Reposiciones.AnyAsync(r => r.Id == id);
        }

        public void DeleteReposicion(Reposicion reposicion)
        {
            _context.Reposiciones.Remove(reposicion);
        }

    }
}
