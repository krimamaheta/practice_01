using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = string.Empty;

        //[Required]
        //[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "the password and confirm password does not match")]
        public string? ConfirmPassword { get; set; } 
        public string? Email { get; set; } 
        public string? Token { get; set; } 
    }
}
