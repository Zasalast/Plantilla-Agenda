using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla_Agenda.Models
{
    public class AgendamientoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgendamiento { get; set; }
        public int? IdCliente { get; set; }
        public string Estado { get; set; }
        public string Servicio { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public string Disponibilidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }


        public DateTime? FechaHora { get; set; }

        // Property to hold the name of the client
        [NotMapped]
        public string? NombreCliente { get; set; }
        public string? NombreProfesional { get; set; }
        

        // Other properties and methods remain unchanged
        // ...
    }
}
