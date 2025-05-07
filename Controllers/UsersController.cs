using ApiGestionMaquinasVirtuales.DTOs;
using ApiGestionMaquinasVirtuales.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiGestionMaquinasVirtuales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsersController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuario = await _usuarioService.CrearUsuarioAsync(dto);
                return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = usuario.Id }, usuario);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            return Ok(usuarios);
        }
    }
}
