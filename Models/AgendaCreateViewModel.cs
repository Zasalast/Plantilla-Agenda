using Microsoft.AspNetCore.Mvc.Rendering;

namespace Plantilla_Agenda.Models
{
    public class AgendaCreateViewModel
    {
        public int IdSede { get; set; }
        public int IdServicio { get; set; }
        public int IdHorario { get; set; }
        public int IdProfesional { get; set; }

        // Otras propiedades según sea necesario

        public SelectList? Sedes { get; set; }
        public SelectList? Servicios { get; set; }
        public SelectList? Horarios { get; set; }
        public SelectList? Profesionales { get; set; }
        public SelectList? Agenda { get; set; }
        public string? PrimerNombreCliente { get; set; }
        public string ProfesionalNombre { get; set; }
        public string PrimerNombreProfesional { get; set; }
        public string PrimerApellidoProfesional { get; set; }
        public string NombreSede { get; set; }
        public string NombreServicio { get; set; }
        public string ClienteNombre { get; set; }
        public string PrimerApellidoCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime HoraInicio { get; set; }
    }
}