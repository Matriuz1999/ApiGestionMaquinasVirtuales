using System;
using System.Collections.Generic;
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
    public class MaquinasVirtualesController : ControllerBase
    {
        private readonly IMaquinaVirtualService _maquinaVirtualService;

        public MaquinasVirtualesController(IMaquinaVirtualService maquinaVirtualService)
        {
            _maquinaVirtualService = maquinaVirtualService;
        }

        // GET: api/MaquinasVirtuales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaquinaVirtualDto>>> GetMaquinasVirtuales()
        {
            try
            {
                var maquinasVirtuales = await _maquinaVirtualService.GetAllAsync();
                if (maquinasVirtuales == null || !maquinasVirtuales.Any())
                {
                    return NoContent(); // 204: No hay contenido que mostrar
                }
                return Ok(maquinasVirtuales);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "Error al obtener las máquinas virtuales.",
                    statusCode: 500);
            }
        }

        // GET: api/MaquinasVirtuales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaquinaVirtualDto>> GetMaquinaVirtual(int id)
        {
            try
            {
                var maquinaVirtual = await _maquinaVirtualService.GetByIdAsync(id);

                if (maquinaVirtual == null)
                {
                    return NotFound(new { message = $"No se encontró la máquina virtual con ID {id}." });
                }

                return Ok(maquinaVirtual);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: $"Error al obtener la máquina virtual con ID {id}.",
                    statusCode: 500);
            }
        }

        // POST: api/MaquinasVirtuales
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<MaquinaVirtualDto>> PostMaquinaVirtual(MaquinaVirtualCreateDto maquinaVirtualDto)
        {
            try
            {
                var createdMaquinaVirtual = await _maquinaVirtualService.CreateAsync(maquinaVirtualDto);
                return CreatedAtAction(nameof(GetMaquinaVirtual), new { id = createdMaquinaVirtual.Id }, createdMaquinaVirtual);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "Error al crear la máquina virtual.",
                    statusCode: 500);
            }
        }

        // PUT: api/MaquinasVirtuales/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PutMaquinaVirtual(int id, MaquinaVirtualUpdateDto maquinaVirtualDto)
        {
            try
            {
                var updatedMaquinaVirtual = await _maquinaVirtualService.UpdateAsync(id, maquinaVirtualDto);

                if (updatedMaquinaVirtual == null)
                {
                    return NotFound(new { message = $"No se pudo actualizar. No se encontró la máquina virtual con ID {id}." });
                }

                return Ok(updatedMaquinaVirtual);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: $"Error al actualizar la máquina virtual con ID {id}.",
                    statusCode: 500);
            }
        }

        // DELETE: api/MaquinasVirtuales/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteMaquinaVirtual(int id)
        {
            try
            {
                var result = await _maquinaVirtualService.DeleteAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"No se encontró la máquina virtual con ID {id} para eliminar." });
                }

                return Ok(new { message = "Máquina virtual eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: $"Error al eliminar la máquina virtual con ID {id}.",
                    statusCode: 500);
            }
        }
    }
}
