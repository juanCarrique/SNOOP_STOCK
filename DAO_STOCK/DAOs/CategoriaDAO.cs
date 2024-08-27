using DataAccess.Context;
using Domain.Models;

namespace DataAccess
{
    public class CategoriaDAO
    {
        private readonly StockappContext _context;

        public CategoriaDAO(StockappContext context)
        {
            _context = context;
        }
        public async Task<Categoria> GetCategoria(int id)
        {
            return await _context.Categoria.FindAsync(id);
        }
    }
}