

using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ClienteDAO
    {
        private readonly StockappContext _context;

        public ClienteDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetCliente(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public void AddCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public void UpdateCliente(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
        }

        public void DeleteCliente(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ClienteExists(int id)
        {
            return await _context.Clientes.AnyAsync(c => c.Id == id);
        }
    }
}