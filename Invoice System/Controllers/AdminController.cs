using AutoMapper;
using Invoice_System.DTOs.Admin;
using Invoice_System.Models;
using Invoice_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoice_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        // ----- USERS -----

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDto);
            var result = await _adminService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetAllUsers), new { id = result.Id }, result);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(updateUserDto);
            user.Id = id; // Set the ID for update
            var updatedUser = await _adminService.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _adminService.DeleteUserAsync(id);
            return NoContent();
        }

        // ----- PRODUCTS -----

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _adminService.GetAllProductsAsync();
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(productDto);
            var result = await _adminService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetAllProducts), new { id = result.Id }, result);
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(updateProductDto);
            product.Id = id; // Set the ID for update
            var updatedProduct = await _adminService.UpdateProductAsync(product);
            return Ok(updatedProduct);
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _adminService.DeleteProductAsync(id);
            return NoContent();
        }

        // ----- INVOICES -----

        [HttpGet("invoices")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _adminService.GetAllInvoicesAsync();
            var invoiceDtos = _mapper.Map<List<InvoiceDto>>(invoices);
            return Ok(invoiceDtos);
        }

        [HttpPost("invoices")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto createInvoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoice = _mapper.Map<Invoice>(createInvoiceDto);
            var result = await _adminService.CreateInvoiceAsync(invoice);
            return CreatedAtAction(nameof(GetAllInvoices), new { id = result.Id }, result);
        }

        [HttpDelete("invoices/{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _adminService.DeleteInvoiceAsync(id);
            return NoContent();
        }
    }
}
