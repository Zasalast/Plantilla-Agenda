using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Plantilla_Agenda.Models
{
    public class Agendamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgendamiento { get; set; }
        public int? IdCliente { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaHora { get; set; }
 

    }
}
