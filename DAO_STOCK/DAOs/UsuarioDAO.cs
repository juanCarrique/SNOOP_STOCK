using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UsuarioDAO
    {
        private readonly StockappContext _context;

        public UsuarioDAO(StockappContext context)
        {
            _context = context;
        }
      
        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public void UpdateUsuario(Usuario u)
        {
            _context.Entry(u).State = EntityState.Modified;
        }

        public void AddUsuario(Usuario u)
        {
            _context.Usuarios.Add(u);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsuarioExists(int id)
        {
            return await _context.Usuarios.AnyAsync(c => c.Id == id);
        }

        public void DeleteUsuario(Usuario u)
        {
            _context.Usuarios.Remove(u);
        }
    }
}
