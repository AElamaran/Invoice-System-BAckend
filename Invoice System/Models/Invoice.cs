using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Invoice_System.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; } // Cashier who created the invoice
        public User User { get; set; }

        public ICollection<InvoiceItem> InvoiceItems { get; set; }

        [Range(0, 100, ErrorMessage = "Discount should be between 0 and 100.")]
        public double DiscountPercentage { get; set; } = 0;

        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Paid amount must be greater than 0.")]
        public decimal PaidAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be zero or more.")]
        public decimal BalanceAmount { get; set; }

    }
}
