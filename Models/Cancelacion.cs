using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class Cancelacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCancelacion { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Motivo { get; set; }
        public int? IdAgendamiento { get; set; }

  
    }
}
