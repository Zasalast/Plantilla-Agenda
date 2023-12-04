namespace Plantilla_Agenda.Models
{
    public class AgendaViewModel
    {
        public int IdAgenda { get; set; }
        public string Estado { get; set; }
        public string ProfesionalNombre { get; set; }
        public string PrimerNombreProfesional { get; set; }
        public string PrimerApellidoProfesional { get; set; }
        public string SedeNombre { get; set; }

        public string ServicioNombre { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public TimeSpan HoraInicio { get; set; }
    }
}
