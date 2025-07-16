using System.ComponentModel.DataAnnotations;

namespace Invoice_System.DTOs.Admin
{
    public class InvoiceItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than 0.")]
        public decimal UnitPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0.")]
        public decimal Total => Quantity * UnitPrice;
    }
}
