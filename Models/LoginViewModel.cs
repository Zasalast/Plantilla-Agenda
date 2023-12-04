using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string ClaveHash { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
    }
}
