using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }
        public string? RoleName { get; set; }

    }
}
