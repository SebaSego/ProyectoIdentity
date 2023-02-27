using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class RecuperaPasswordViewModel
    {
        [Required(ErrorMessage = "Email requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña obligatoria")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coindice con la ingresada")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
