using System.Threading.Tasks;
using ApiGestionMaquinasVirtuales.DTOs;
using ApiGestionMaquinasVirtuales.Hubs;
using ApiGestionMaquinasVirtuales.Models;
using Microsoft.AspNetCore.SignalR;

namespace ApiGestionMaquinasVirtuales.Hubs
{
    public class MaquinasVirtualesHubService
    {
        private readonly IHubContext<MaquinasVirtualesHub> _hubContext;

        public MaquinasVirtualesHubService(IHubContext<MaquinasVirtualesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificarCreacionAsync(MaquinaVirtualDto maquinaVirtual)
        {
            await _hubContext.Clients.All.SendAsync("MaquinaVirtualCreada", "Se creo maquina virtual con nombre:" + maquinaVirtual.Nombre);
        }

        public async Task NotificarActualizacionAsync(MaquinaVirtualDto maquinaVirtual)
        {
            await _hubContext.Clients.All.SendAsync("MaquinaVirtualActualizada", "Se actualizo maquina virtual con nombre:" + maquinaVirtual.Nombre);
        }

        public async Task NotificarEliminacionAsync(int id)
        {
            await _hubContext.Clients.All.SendAsync("MaquinaVirtualEliminada", "Se elimino maquina virtual con id:" + id);
        }
    }
}