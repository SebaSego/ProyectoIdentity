using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class AccesoViewModel
    {
        [Required(ErrorMessage ="Email requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Contraseña obligatoria")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [DisplayName("Recordar datos?")]
        public bool RememberMe { get; set; }   

        

    }
}
