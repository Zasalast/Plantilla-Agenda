using Microsoft.AspNetCore.Mvc.Rendering;

namespace Plantilla_Agenda.Models
{
    public class AgendaCreateViewModel
    {
        public int IdPersona { get; set; }

        public int? IdAgenda { get; set; }

        public string? PrimerNombre { get; set; }

        public string? SegundoNombre { get; set; }

        public string? PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        public int IdProfesional { get; set; }

        public int? IdAgendamiento { get; set; }

        public int IdCliente { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public int? IdAsistencia { get; set; }

        public char? EstadoAsistencia { get; set; }

        public int? IdCancelacion { get; set; }

        public int IdHorario { get; set; }

        public TimeSpan? HoraFin { get; set; }

        public int IdSede { get; set; }

        public string? Direccion { get; set; }

        public int IdServicio { get; set; }

        public string? Nombre { get; set; }

        public int IdUsuario { get; set; }

        public string? UserName { get; set; }

        public SelectList? Sedes { get; set; }

        public SelectList? Servicios { get; set; }

        public SelectList? Horarios { get; set; }

        public SelectList? Personas { get; set; }

        public string NombreSede { get; set; }

        public string NombreServicio { get; set; }

        public TimeSpan? HoraInicioHorario { get; set; }

        public string NombreProfesional { get; set; }
    }
}