using Invoice_System.DTOs.Admin;

namespace Invoice_System.DTOs.cashier
{
    public class InvoiceDetailDto
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }  // Cashier name (from User)

        public List<InvoiceItemDto> InvoiceItems { get; set; }

        public double DiscountPercentage { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal BalanceAmount { get; set; }
    }
}
