using System.ComponentModel.DataAnnotations;

namespace StaffScreening.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
