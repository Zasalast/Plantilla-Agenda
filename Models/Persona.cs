using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla_Agenda.Models
{
    public class Persona
    {
        public int IdPersona { get; set; }


        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public string TipoDocumento { get; set; }
        public string Identificacion { get; set; }     
        public int IdRol { get; set; }
        public string NombreCompleto { get { return string.Format("{0} {1}", PrimerNombre, PrimerApellido); } }
        [NotMapped]
        public bool MantenerActivo { get; set; }
    }
}
