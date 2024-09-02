using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{   
    public class LoginService
    {
        private readonly StockappContext _context;
        public LoginService(StockappContext context)
        {
            _context = context;
        }
    
        public async Task<Usuario> GetUsuario(UsuarioLoginDTO usuario)
        {
            return await _context.Usuarios.
                SingleOrDefaultAsync(x => x.Mail == usuario.Mail && x.Password == usuario.Password);
        }
    }

}
