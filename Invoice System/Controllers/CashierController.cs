using AutoMapper;
using Invoice_System.DTOs.cashier;
using Invoice_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoice_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class CashierController : ControllerBase
    {
        private readonly ICashierService _cashierService;
        private readonly IMapper _mapper;

        public CashierController(ICashierService cashierService, IMapper mapper)
        {
            _cashierService = cashierService;
            _mapper = mapper;
        }

        // ----- CREATE INVOICE -----
        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto createInvoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoice = await _cashierService.CreateInvoiceAsync(createInvoiceDto);
            var invoiceDetailDto = _mapper.Map<InvoiceDetailDto>(invoice);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoiceDetailDto);
        }

        // ----- VIEW INVOICE -----
        [HttpGet("invoice/{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            var invoice = await _cashierService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            var invoiceDetailDto = _mapper.Map<InvoiceDetailDto>(invoice);
            return Ok(invoiceDetailDto);
        }
    }
}
