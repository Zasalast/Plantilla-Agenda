namespace Plantilla_Agenda.Models
{
    public class AgendaDetails
    {
        public int IdAgenda { get; set; }

        public int IdProfesional { get; set; }

        public int IdHorario { get; set; }

        public int IdSede { get; set; }

        public int IdServicio { get; set; }

        public string ProfesionalNombre { get; set; }

        public string SedeDireccion { get; set; }

        public string ServicioNombre { get; set; }

        public DateTime HorarioInicio { get; set; }

        public DateTime HorarioFin { get; set; }

        public string Estado { get; set; }

        public List<Persona> Persona { get; set; }
        public List<Servicio> Servicio { get; set; }
        public List<Sede> Sede { get; set; }
        public List<Horario> Horario { get; set; }
        public List<Agenda> Agenda { get; set; }
    }
}