using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; } 

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
