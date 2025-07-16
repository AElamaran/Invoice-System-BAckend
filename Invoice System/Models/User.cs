using System.ComponentModel.DataAnnotations;

namespace Invoice_System.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Store hashed password

        [Required]
        [RegularExpression("^(Admin|Cashier)$", ErrorMessage = "Role must be either 'Admin' or 'Cashier'")]
        public string Role { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
