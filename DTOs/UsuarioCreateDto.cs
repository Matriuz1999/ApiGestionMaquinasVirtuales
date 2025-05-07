using System.ComponentModel.DataAnnotations;

namespace ApiGestionMaquinasVirtuales.DTOs
{
    public class UsuarioCreateDto
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Rol { get; set; }
    }
}