using Microsoft.AspNetCore.Mvc.Rendering;

namespace Plantilla_Agenda.Models
{
    public class AgendaCreateViewModel
    {
        public int? IdSede { get; set; }
        public int? IdServicio { get; set; }
        public int IdHorario { get; set; }
        public int IdProfesional { get; set; }

        // Otras propiedades según sea necesario

        public SelectList? Sedes { get; set; }
        public SelectList? Servicios { get; set; }
        public SelectList? Horarios { get; set; }
        public SelectList? Profesionales { get; set; }
        public SelectList? Personas { get; set; }
        // Resto del modelo
    }
}
