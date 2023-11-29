﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class Servicios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdServicio { get; set; }
        public string Nombre { get; set; }
    }
}
