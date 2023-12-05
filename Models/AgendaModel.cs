using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla_Agenda.Models
{
    [Table("Agenda")]
    public class AgendaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgenda { get; set; }

        public int? IdSede { get; set; }

        public int? IdServicio { get; set; }

        public int? IdHorario { get; set; }

        public int? IdProfesional { get; set; }

        public string? Estado { get; set; }

        public int? IdCliente { get; set; }

        public int? IdSedeAgendada { get; set; }

        public int? IdServicioAgendado { get; set; }

        [NotMapped]
        public string? PrimerNombreCliente { get; set; }

        public string? ProfesionalNombre { get; set; }

        public string? PrimerNombreProfesional { get; set; }

        public string? PrimerApellidoProfesional { get; set; }

        public string? NombreSede { get; set; }

        public string? NombreServicio { get; set; }

        public string? ClienteNombre { get; set; }

        public string? PrimerApellidoCliente { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? HoraInicio { get; set; }

        public string? SedeDireccion { get; set; }
     
    }
}
