using System;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.Models;
using ApiGestionMaquinasVirtuales.DTOs;

namespace ApiGestionMaquinasVirtuales.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(Usuario usuario);
        bool ValidateToken(string token);
    }
}