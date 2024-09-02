using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class TipoMovimientoDAO
    {
        private readonly StockappContext _context;

        public TipoMovimientoDAO(StockappContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TipoMovimiento>> GetTipoMovimientos()
        {
            return await _context.TipoMovimientos.ToListAsync();
        }
        public async Task<TipoMovimiento> GetTipoMovimiento(int id)
        {
            return await _context.TipoMovimientos.FindAsync(id);
        }
        public void UpdateTipoMovimiento(TipoMovimiento tm)
        {
            _context.Entry(tm).State = EntityState.Modified;
        }
        public void AddTipoMovimiento(TipoMovimiento tm)
        {
            _context.TipoMovimientos.Add(tm);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> TipoMovimientoExists(int id)
        {
            return await _context.TipoMovimientos.AnyAsync(c => c.Id == id);
        }
        public void DeleteTipoMovimiento(TipoMovimiento tm)
        {
            _context.TipoMovimientos.Remove(tm);
        }
    }
}
