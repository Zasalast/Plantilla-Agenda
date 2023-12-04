using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Agenda.Models
{
    public class RolPermisoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        // Propiedades de navegación
        public RolModel Rol { get; set; }
        public PermisoModel Permiso { get; set; }

    }
}
