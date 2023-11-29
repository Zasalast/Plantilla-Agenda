using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class Agendamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgendamiento { get; set; }
        public int? IdCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public char? Estado { get; set; }
        public int? IdAgenda { get; set; }

        public int? IdCancelacion { get; set; }
        public int? IdReserva { get; set; }

    }
}
