using System;

namespace ApiGestionMaquinasVirtuales.DTOs
{
    public class MaquinaVirtualDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cores { get; set; }
        public int RAM { get; set; }
        public int Disco { get; set; }
        public string OS { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
