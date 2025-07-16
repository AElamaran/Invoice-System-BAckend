using System.ComponentModel.DataAnnotations;

namespace Invoice_System.DTOs.Admin
{
    public class ProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }

    public class UpdateProductDto : ProductDto
    {
        [Required]
        public int Id { get; set; }
    }
}
