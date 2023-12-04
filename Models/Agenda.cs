using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    [Table("Agenda")]
    public class Agenda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgenda { get; set; }

        public int? IdSede { get; set; }

        public int? IdServicio { get; set; }

        public int? IdHorario { get; set; }

        public int? IdProfesional { get; set; }

        public string Estado { get; set; }

        public int? IdCliente { get; set; }

        public int? IdSedeAgendada { get; set; }

        public int? IdServicioAgendado { get; set; }
    }
}