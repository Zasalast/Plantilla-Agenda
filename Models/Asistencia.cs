﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class Asistencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAsistencia { get; set; }
        public string EstadoAsistencia { get; set; }
        public int? IdAgendamiento { get; set; }

        // Propiedades de navegación
        public Agendamiento Agendamiento { get; set; }
    }
}
