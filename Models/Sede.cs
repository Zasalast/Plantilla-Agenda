using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Plantilla_Agenda.Models
{
    public class Sede
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSede { get; set; }
        public string Direccion { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public TimeSpan Duracion { get; set; }
        public string Estado { get; set; }
    }
}
