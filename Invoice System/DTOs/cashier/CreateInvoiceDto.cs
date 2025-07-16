using Invoice_System.DTOs.Admin;
using System.ComponentModel.DataAnnotations;

namespace Invoice_System.DTOs.cashier
{
    public class CreateInvoiceDto
    {
        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }

        [Required]
        public List<InvoiceItemDto> InvoiceItems { get; set; }

        [Range(0, 100, ErrorMessage = "Discount should be between 0 and 100.")]
        public double DiscountPercentage { get; set; } // No semicolon needed here.

        [Range(0.01, double.MaxValue, ErrorMessage = "Paid amount must be greater than 0.")]
        public decimal PaidAmount { get; set; }
    }
}
