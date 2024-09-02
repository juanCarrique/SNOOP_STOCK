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
    public class HistorialMovimientoDAO
    {
        private readonly StockappContext _context;

        public HistorialMovimientoDAO(StockappContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<HistorialMovimiento>> GetHistorialMovimientos()
        {
            return await _context.HistorialMovimientos.ToListAsync();
        }
        public async Task<HistorialMovimiento> GetHistorialMovimiento(int id)
        {
            return await _context.HistorialMovimientos.FindAsync(id);
        }
        public void UpdateHistorialMovimiento(HistorialMovimiento hm)
        {
            _context.Entry(hm).State = EntityState.Modified;
        }
        public void AddHistorialMovimiento(HistorialMovimiento hm)
        {
            _context.HistorialMovimientos.Add(hm);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HistorialMovimientoExists(int id)
        {
            return await _context.HistorialMovimientos.AnyAsync(c => c.Id == id);
        }
        public void DeleteHistorialMovimiento(HistorialMovimiento hm)
        {
            _context.HistorialMovimientos.Remove(hm);
        }
    }
}
