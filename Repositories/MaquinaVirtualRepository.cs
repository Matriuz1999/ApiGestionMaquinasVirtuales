using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.Interfaces;
using ApiGestionMaquinasVirtuales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiGestionMaquinasVirtuales.Repositories
{
    public class MaquinaVirtualRepository : IMaquinaVirtualRepository
    {
        private readonly GestionmvContext _context;

        public MaquinaVirtualRepository(GestionmvContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaquinasVirtuale>> GetAllAsync()
        {
            return await _context.MaquinasVirtuales.ToListAsync();
        }

        public async Task<MaquinasVirtuale> GetByIdAsync(int id)
        {
            return await _context.MaquinasVirtuales.FindAsync(id);
        }

        public async Task<MaquinasVirtuale> CreateAsync(MaquinasVirtuale maquinaVirtual)
        {
            _context.MaquinasVirtuales.Add(maquinaVirtual);
            await _context.SaveChangesAsync();
            return maquinaVirtual;
        }

        public async Task<MaquinasVirtuale> UpdateAsync(int id, MaquinasVirtuale maquinaVirtual)
        {
            var existingMaquinaVirtual = await _context.MaquinasVirtuales.FindAsync(id);
            if (existingMaquinaVirtual == null)
            {
                return null;
            }

            _context.Entry(existingMaquinaVirtual).CurrentValues.SetValues(maquinaVirtual);
            await _context.SaveChangesAsync();
            return existingMaquinaVirtual;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var maquinaVirtual = await _context.MaquinasVirtuales.FindAsync(id);
            if (maquinaVirtual == null)
            {
                return false;
            }

            _context.MaquinasVirtuales.Remove(maquinaVirtual);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}