using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.DTOs;
using ApiGestionMaquinasVirtuales.Models;

namespace ApiGestionMaquinasVirtuales.Interfaces
{
    public interface IMaquinaVirtualService
    {
        Task<IEnumerable<MaquinaVirtualDto>> GetAllAsync();
        Task<MaquinaVirtualDto> GetByIdAsync(int id);
        Task<MaquinaVirtualDto> CreateAsync(MaquinaVirtualCreateDto maquinaVirtualDto);
        Task<MaquinaVirtualDto> UpdateAsync(int id, MaquinaVirtualUpdateDto maquinaVirtualDto);
        Task<bool> DeleteAsync(int id);
    }
}