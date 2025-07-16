using AutoMapper;
using Invoice_System.Data;
using Invoice_System.DTOs.cashier;
using Invoice_System.Services;
using Microsoft.EntityFrameworkCore;
using Invoice_System.Models;
using System;

namespace Invoice_System.Services
{
    public class CashierService : ICashierService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CashierService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Create a new invoice
        public async Task<Invoice> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
        {
            // Start a database transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            decimal total = 0;

            // Validate stock availability for each product in the invoice
            foreach (var item in createInvoiceDto.InvoiceItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                if (product.Stock < item.Quantity)
                    throw new Exception($"Not enough stock for product '{product.Name}'. Available: {product.Stock}");

                // Deduct stock
                product.Stock -= item.Quantity;

                // Calculate item total
                item.UnitPrice = product.UnitPrice;
                total += item.UnitPrice * item.Quantity;
            }

            // Apply discount if any
            decimal discount = ((decimal)createInvoiceDto.DiscountPercentage / 100m) * total;

            decimal totalAmount = total - discount;
            decimal balanceAmount = totalAmount - createInvoiceDto.PaidAmount;

            // Create the invoice object
            var invoice = new Invoice
            {
                UserId = createInvoiceDto.UserId,
                TransactionDate = createInvoiceDto.TransactionDate,
                DiscountPercentage = createInvoiceDto.DiscountPercentage,
                TotalAmount = totalAmount,
                PaidAmount = createInvoiceDto.PaidAmount,
                BalanceAmount = balanceAmount,
                InvoiceItems = _mapper.Map<List<InvoiceItem>>(createInvoiceDto.InvoiceItems)
            };

            // Add the invoice to the database
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            // Return the created invoice
            return invoice;
        }

        // Get invoice details by ID
        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.User)
                .Include(i => i.InvoiceItems)
                .ThenInclude(ii => ii.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
