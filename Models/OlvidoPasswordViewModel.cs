using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class OlvidoPasswordViewModel
    {
        [Required(ErrorMessage ="Email requerido")]
        public string Email { get; set; }

        

        

    }
}
