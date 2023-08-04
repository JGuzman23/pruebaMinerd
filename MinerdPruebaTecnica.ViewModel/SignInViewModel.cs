using System.ComponentModel.DataAnnotations;

namespace MinerdPruebaTecnica.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
