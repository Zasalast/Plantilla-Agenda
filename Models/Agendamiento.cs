using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantilla_Agenda.Models
{
    public class Agendamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgendamiento { get; set; }
        public int? IdCliente { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaHora { get; set; }

        // Property to hold the name of the client
        [NotMapped]
        public string? NombreCliente { get; set; }

        // Other properties and methods remain unchanged
        // ...
    }
}
