using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla_Agenda.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? UserName { get; set; }
        public string? Clave { get; set; }

        [NotMapped]
        public bool MantenerActivo { get; set; }
    }
}
