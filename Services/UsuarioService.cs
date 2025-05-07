using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.DTOs;
using ApiGestionMaquinasVirtuales.Interfaces;
using ApiGestionMaquinasVirtuales.Models;

namespace ApiGestionMaquinasVirtuales.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioDto> CrearUsuarioAsync(UsuarioCreateDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.Password, // El repo lo hashea
                Rol = dto.Rol
            };

            var creado = await _usuarioRepository.CreateAsync(usuario);

            return new UsuarioDto
            {
                Id = creado.Id,
                Nombre = creado.Nombre,
                Email = creado.Email,
                Rol = creado.Rol
            };
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();

            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol
            });
        }

        public async Task<UsuarioDto> ObtenerPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            if (usuario == null) return null;

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }
    }
}
