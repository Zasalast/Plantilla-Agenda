using System;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class CreateAgendaModel
    {
        // Propiedades requeridas para crear una nueva agenda
        [Required(ErrorMessage = "El campo IdSede es obligatorio.")]
        public int IdSede { get; set; }

        [Required(ErrorMessage = "El campo IdServicio es obligatorio.")]
        public int IdServicio { get; set; }

        [Required(ErrorMessage = "El campo IdHorario es obligatorio.")]
        public int IdHorario { get; set; }

        [Required(ErrorMessage = "El campo IdProfesional es obligatorio.")]
        public int IdProfesional { get; set; }

        // Otras propiedades necesarias para la creación
        // Puedes agregar más propiedades según tus necesidades
    }
}
