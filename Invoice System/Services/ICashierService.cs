using Invoice_System.DTOs.cashier;
using Invoice_System.Models;

namespace Invoice_System.Services
{
    public interface ICashierService
    {
        Task<Invoice> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);
        Task<Invoice> GetInvoiceByIdAsync(int id);
    }
}
