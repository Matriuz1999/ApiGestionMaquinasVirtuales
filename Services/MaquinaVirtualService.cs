using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.DTOs;
using ApiGestionMaquinasVirtuales.Hubs;
using ApiGestionMaquinasVirtuales.Interfaces;
using ApiGestionMaquinasVirtuales.Models;

namespace ApiGestionMaquinasVirtuales.Services
{
    public class MaquinaVirtualService : IMaquinaVirtualService
    {
        private readonly IMaquinaVirtualRepository _repository;
        private readonly MaquinasVirtualesHubService _hubService;

        public MaquinaVirtualService(IMaquinaVirtualRepository repository, MaquinasVirtualesHubService hubService)
        {
            _repository = repository;
            _hubService = hubService;
        }

        public async Task<IEnumerable<MaquinaVirtualDto>> GetAllAsync()
        {
            var maquinasVirtuales = await _repository.GetAllAsync();
            return maquinasVirtuales.Select(MapToDto);
        }

        public async Task<MaquinaVirtualDto> GetByIdAsync(int id)
        {
            var maquinaVirtual = await _repository.GetByIdAsync(id);
            return maquinaVirtual != null ? MapToDto(maquinaVirtual) : null;
        }

        public async Task<MaquinaVirtualDto> CreateAsync(MaquinaVirtualCreateDto maquinaVirtualDto)
        {
            var maquinaVirtual = new MaquinasVirtuale
            {
                Nombre = maquinaVirtualDto.Nombre,
                Cores = maquinaVirtualDto.Cores,
                Ram = maquinaVirtualDto.RAM,
                Disco = maquinaVirtualDto.Disco,
                Os = maquinaVirtualDto.OS,
                Estado = maquinaVirtualDto.Estado,
                FechaCreacion = DateTime.Now
            };

            var createdMaquinaVirtual = await _repository.CreateAsync(maquinaVirtual);
            var maquinaVirtualCreatedDto = MapToDto(createdMaquinaVirtual);

            // Notificar a los clientes sobre la creación mediante SignalR
            await _hubService.NotificarCreacionAsync(maquinaVirtualCreatedDto);

            return maquinaVirtualCreatedDto;
        }

        public async Task<MaquinaVirtualDto> UpdateAsync(int id, MaquinaVirtualUpdateDto maquinaVirtualDto)
        {
            var existingMaquinaVirtual = await _repository.GetByIdAsync(id);

            if (existingMaquinaVirtual == null)
            {
                return null;
            }

            existingMaquinaVirtual.Cores = maquinaVirtualDto.Cores;
            existingMaquinaVirtual.Ram = maquinaVirtualDto.RAM;
            existingMaquinaVirtual.Disco = maquinaVirtualDto.Disco;
            existingMaquinaVirtual.Os = maquinaVirtualDto.OS;
            existingMaquinaVirtual.Estado = maquinaVirtualDto.Estado;
            existingMaquinaVirtual.FechaActualizacion = DateTime.Now;

            var updatedMaquinaVirtual = await _repository.UpdateAsync(id, existingMaquinaVirtual);
            var maquinaVirtualUpdatedDto = MapToDto(updatedMaquinaVirtual);

            // Notificar a los clientes sobre la actualización mediante SignalR
            await _hubService.NotificarActualizacionAsync(maquinaVirtualUpdatedDto);

            return maquinaVirtualUpdatedDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (result)
            {
                // Notificar a los clientes sobre la eliminación mediante SignalR
                await _hubService.NotificarEliminacionAsync(id);
            }

            return result;
        }

        private MaquinaVirtualDto MapToDto(MaquinasVirtuale maquinaVirtual)
        {
            return new MaquinaVirtualDto
            {
                Id = maquinaVirtual.Id,
                Nombre = maquinaVirtual.Nombre,
                Cores = maquinaVirtual.Cores,
                RAM = maquinaVirtual.Ram,
                Disco = maquinaVirtual.Disco,
                OS = maquinaVirtual.Os,
                Estado = maquinaVirtual.Estado,
                FechaCreacion = maquinaVirtual.FechaCreacion,
                FechaActualizacion = maquinaVirtual.FechaActualizacion
            };
        }
    }
}