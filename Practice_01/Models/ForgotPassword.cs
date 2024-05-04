using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    public class ForgotPassword
    {

        //[Required]
        [EmailAddress]
        public string? Email { get; set; } 
    }
}
