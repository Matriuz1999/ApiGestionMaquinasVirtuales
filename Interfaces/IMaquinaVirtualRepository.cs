using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.Models;

namespace ApiGestionMaquinasVirtuales.Interfaces
{
    public interface IMaquinaVirtualRepository
    {
        Task<IEnumerable<MaquinasVirtuale>> GetAllAsync();
        Task<MaquinasVirtuale> GetByIdAsync(int id);
        Task<MaquinasVirtuale> CreateAsync(MaquinasVirtuale maquinaVirtual);
        Task<MaquinasVirtuale> UpdateAsync(int id, MaquinasVirtuale maquinaVirtual);
        Task<bool> DeleteAsync(int id);
    }
}