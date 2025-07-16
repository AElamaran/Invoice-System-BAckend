using Invoice_System.Models;
using System.ComponentModel.DataAnnotations;

public enum UserRole
{
    Admin,
    Cashier
}

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public UserRole Role { get; set; }  // ✅ Enum type here

    public ICollection<Invoice> Invoices { get; set; }
}
