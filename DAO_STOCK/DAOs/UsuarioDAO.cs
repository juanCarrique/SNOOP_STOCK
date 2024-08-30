using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UsuarioDAO
    {
        private readonly StockappContext _context;

        public UsuarioDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuario(int vendedorId)
        {
            return await _context.Usuarios.FindAsync(vendedorId);
        }
    }
}
