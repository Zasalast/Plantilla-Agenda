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
      
        public char? Estado { get; set; }
        public int? IdAgenda { get; set; }

        public int? IdCancelacion { get; set; }
        public int? IdReserva { get; set; }
        public List<Cancelacion>? Cancelaciones { get; set; }
        public List<Agenda>? Agendas { get; set; }
        public List<Persona>? personas { get; set; }

    }
}
