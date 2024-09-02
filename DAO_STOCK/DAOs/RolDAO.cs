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
    public class RolDAO
    {
        private readonly StockappContext _context;

        public RolDAO(StockappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Rol> GetRol(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
        public void UpdateRol(Rol r)
        {
            _context.Entry(r).State = EntityState.Modified;
        }

        public void AddRol(Rol r)
        {
            _context.Roles.Add(r);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RolExists(int id)
        {
            return await _context.Roles.AnyAsync(c => c.Id == id);
        }

        public void DeleteRol(Rol r)
        {
            _context.Roles.Remove(r);
        }
    }
}
