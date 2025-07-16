namespace Invoice_System.DTOs.Admin
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int UserId { get; set; }

        public List<InvoiceItemDto> InvoiceItems { get; set; }

        public double DiscountPercentage { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal BalanceAmount { get; set; }
    }

    public class CreateInvoiceDto : InvoiceDto
    {
        public List<InvoiceItemDto> InvoiceItems { get; set; }
    }
}
