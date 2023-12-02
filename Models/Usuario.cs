﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveHash { get; set; }
        public int IdPersona { get; set; }
        public int? IdRol { get; set; }
        public string Activo { get; set; }

        // Propiedades de navegación
        public Persona Persona { get; set; }
        public Rol Rol { get; set; }

        [NotMapped]
        public bool MantenerActivo { get; set; }
    }
}
