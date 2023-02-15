using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage ="Email requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Contraseña obligatoria")]
        [StringLength(50, ErrorMessage ="El {0} debe ser mayor a {2} caracteres de longitud", MinimumLength =5)]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage="La contraseña no coindice con la ingresada")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ingrese su nombre")]

        public string Nombre { get; set; }
        public string Url { get; set; }
        public int CodigoPais { get; set; }
        public string Telefono { get; set; }
        
        [Required(ErrorMessage = "Ingrese país")]
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Fecha Obligatorio")]
        [DisplayName("Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Estado Obligatorio")]
        public bool Estado { get; set; }

    }
}
