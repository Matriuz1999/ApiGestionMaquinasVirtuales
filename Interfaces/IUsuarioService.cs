using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.DTOs;

namespace ApiGestionMaquinasVirtuales.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> CrearUsuarioAsync(UsuarioCreateDto dto);
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();
        Task<UsuarioDto> ObtenerPorIdAsync(int id);
    }
}
