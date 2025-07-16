using Invoice_System.Models;

namespace Invoice_System.Services
{
    public interface IAdminService
    {
        // ----- USER FUNCTIONS -----
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        // ----- PRODUCT FUNCTIONS -----
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);

        // ----- INVOICE FUNCTIONS -----
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(int id);
    }
}
