using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.Interfaces;
using ApiGestionMaquinasVirtuales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

namespace ApiGestionMaquinasVirtuales.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GestionmvContext _context;

        public UsuarioRepository(GestionmvContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            // Verificar si ya existe un usuario con el mismo email
            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
            if (existingUser != null)
            {
                throw new Exception("Ya existe un usuario con este email");
            }

            // Hash de la contraseña antes de guardarla
            usuario.PasswordHash = BC.HashPassword(usuario.PasswordHash);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> UpdateAsync(int id, Usuario usuario)
        {
            var existingUsuario = await _context.Usuarios.FindAsync(id);

            if (existingUsuario == null)
            {
                return null;
            }

            // Si se está actualizando el email, verificar que no exista otro usuario con ese email
            if (existingUsuario.Email != usuario.Email)
            {
                var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.Id != id);
                if (emailExists)
                {
                    throw new Exception("Ya existe un usuario con este email");
                }
            }

            // Si se está actualizando la contraseña, hashearla
            if (!string.IsNullOrEmpty(usuario.PasswordHash) && usuario.PasswordHash != existingUsuario.PasswordHash)
            {
                usuario.PasswordHash = BC.HashPassword(usuario.PasswordHash);
            }

            _context.Entry(existingUsuario).CurrentValues.SetValues(usuario);
            await _context.SaveChangesAsync();

            return existingUsuario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return false;
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return false;
            }

            return BC.Verify(password, usuario.PasswordHash);
        }
    }
}