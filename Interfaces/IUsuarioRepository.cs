using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.Models;

namespace ApiGestionMaquinasVirtuales.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<Usuario> UpdateAsync(int id, Usuario usuario);
        Task<bool> DeleteAsync(int id);
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }
}