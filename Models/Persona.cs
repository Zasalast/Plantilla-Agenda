using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla_Agenda.Models
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string? UserName { get; set; }
        public string? Clave { get; set; }        
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public string TipoDocumento { get; set; }
        public string Identificacion { get; set; }     
        public int IdRol { get; set; }
        
        [NotMapped]
        public bool MantenerActivo { get; set; }
    }
}
