using DATA_STOCK.Context;
using DATA_STOCK.Models;
using Microsoft.EntityFrameworkCore;

namespace DAO_STOCK
{
    public class ProductoDAO
    {
        private readonly StockappContext _context;

        public ProductoDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetProductos()
        {
            return await _context.Productos.Include(p => p.Categoria).ToListAsync();
        }

        public async Task<Producto> GetProducto(int id)
        {
            #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Productos.FindAsync(id);
            #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public void AddProducto(Producto producto)
        {
            _context.Productos.Add(producto);
        }

        public void UpdateProducto(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductoExists(int id)
        {
            return await _context.Productos.AnyAsync(p => p.Id == id);
        }

        public void DeleteProducto(Producto producto)
        {
            _context.Productos.Remove(producto);
        }
    }
}   
