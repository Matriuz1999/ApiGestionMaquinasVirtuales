using System;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.DTOs;
using ApiGestionMaquinasVirtuales.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionMaquinasVirtuales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [AllowAnonymous] 
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);

                if (response == null)
                {
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}