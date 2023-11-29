using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class Agenda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgenda { get; set; }
        public int? IdSede { get; set; }
        public int? IdServicio { get; set; }
        public int? IdHorario { get; set; }
        public int? IdProfesional { get; set; }


    }
}
