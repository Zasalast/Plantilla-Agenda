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
        public int IdPersona { get; set; }

        public string? PrimerNombre { get; set; }
      
        public int? IdAgendamiento { get; set; }
        public int IdCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public int? IdAsistencia { get; set; }
        public char? EstadoAsistencia { get; set; }
        public int? IdCancelacion { get; set; }
        public DateTime? FechaHora { get; set; }
       
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        
        public string? Direccion { get; set; }
       
        public string? Nombre { get; set; }
        public int IdUsuario { get; set; }
        public string? UserName { get; set; }
        public SelectList? Sedes { get; set; }
        public SelectList? Servicios { get; set; }
        public SelectList? Horarios { get; set; }
        public SelectList? Profesionales { get; set; }
        public SelectList? Personas { get; set; }
        // Resto del modelo
    }
}
