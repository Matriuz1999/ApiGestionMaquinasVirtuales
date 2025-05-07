using System;
namespace ApiGestionMaquinasVirtuales.DTOs
{
    public class MaquinaVirtualUpdateDto
    {
        public int Cores { get; set; }
        public int RAM { get; set; }
        public int Disco { get; set; }
        public string OS { get; set; }
        public string Estado { get; set; }
    }
}