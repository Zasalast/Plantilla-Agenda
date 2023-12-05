namespace Plantilla_Agenda.Models
{
    public class AgendaDetailsModel
    {
        public int IdAgenda { get; set; }

        public int IdProfesional { get; set; }

        public int IdHorario { get; set; }

        public int? IdSede { get; set; }

        public int? IdServicio { get; set; }

        public string? ProfesionalNombre { get; set; }

        public string? SedeDireccion { get; set; }

        public string? ServicioNombre { get; set; }

        public DateTime? HorarioInicio { get; set; }

        public DateTime? HorarioFin { get; set; }

        public string? Estado { get; set; }

        public List<PersonaModel>? Persona { get; set; }
        public List<ServicioModel>? Servicio { get; set; }
        public List<SedeModel>? Sede { get; set; }
        public List<HorarioModel>? Horario { get; set; }
        public List<AgendaModel>? Agenda { get; set; }
    }
}