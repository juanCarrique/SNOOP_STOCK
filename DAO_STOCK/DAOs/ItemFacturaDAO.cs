using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ItemFacturaDAO
    {
        private readonly StockappContext _context;

        public ItemFacturaDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemFactura>> GetItemsFactura()
        {
            return await _context.ItemsFactura
                .Include(i => i.Producto)
                .ToListAsync();
        }

        public async Task<ItemFactura> GetItemFactura(int id)
        {
            return await _context.ItemsFactura
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<ItemFactura>> GetItemsFacturaByFacturaId(int facturaId)
        {
            return await _context.ItemsFactura
                .Include(i => i.Producto)
                .Where(i => i.FacturaId == facturaId)
                .ToListAsync();
        }

        public void AddItemFactura(ItemFactura itemFactura)
        {
            _context.ItemsFactura.Add(itemFactura);
        }

        public void UpdateItemFactura(ItemFactura itemFactura)
        {
            _context.Entry(itemFactura).State = EntityState.Modified;
        }

        public void DeleteItemFactura(ItemFactura itemFactura)
        {
            _context.ItemsFactura.Remove(itemFactura);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ItemFacturaExists(int id)
        {
            return await _context.ItemsFactura.AnyAsync(i => i.Id == id);
        }
    }
}
