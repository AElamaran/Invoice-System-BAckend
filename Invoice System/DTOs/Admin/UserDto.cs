using System.ComponentModel.DataAnnotations;

namespace Invoice_System.DTOs.Admin
{
    public class UserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [RegularExpression("^(Admin|Cashier)$", ErrorMessage = "Role must be either 'Admin' or 'Cashier'")]
        public string Role { get; set; }
    }

    public class UpdateUserDto : UserDto
    {
        [Required]
        public int Id { get; set; }
    }
}
