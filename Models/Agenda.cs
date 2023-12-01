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
        public int? Estado { get; set; }

        public List<Sede>? Sedes { get; set; }
        public List<Servicio>? Servicioss { get; set; }
        public List<Horario>? Horarios { get; set; }
        public List<Persona>? Personas { get; set; }
        public Sede Sede { get; set; }
        public Servicio Servicio { get; set; }
        public Horario Horario { get; set; }
        public Persona Profesional { get; set; }
    }
}
