using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CategoriaDAO
    {
        private readonly StockappContext _context;

        public CategoriaDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetCategorias()
        {
            return await _context.Categoria.ToListAsync();
        }

        public async Task<Categoria> GetCategoria(int id)
        {
            return await _context.Categoria.FindAsync(id);
        }

        public void UpdateCategoria(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
        }

        public void AddCategoria(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CategoriaExists(int id)
        {
            return await _context.Categoria.AnyAsync(c => c.Id == id);
        }

        public void DeleteCategoria(Categoria categoria)
        {
            _context.Categoria.Remove(categoria);
        }
    }
}