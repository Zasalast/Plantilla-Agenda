using System;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class UpdateAgendaModel
    {
        // Propiedades requeridas para actualizar una agenda existente
        [Required(ErrorMessage = "El campo IdAgenda es obligatorio.")]
        public int IdAgenda { get; set; }

        [Required(ErrorMessage = "El campo IdSede es obligatorio.")]
        public int IdSede { get; set; }

        [Required(ErrorMessage = "El campo IdServicio es obligatorio.")]
        public int IdServicio { get; set; }

        [Required(ErrorMessage = "El campo IdHorario es obligatorio.")]
        public int IdHorario { get; set; }

        [Required(ErrorMessage = "El campo IdProfesional es obligatorio.")]
        public int IdProfesional { get; set; }

        // Otras propiedades necesarias para la actualización
        // Puedes agregar más propiedades según tus necesidades
    }
}
