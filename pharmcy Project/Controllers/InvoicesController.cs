using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmcy.Repository.Data;
using Pharmcy.Core.Entities;
using Pharmcy.Core.Repositories;
using pharmcy_Project.DTO;
using pharmcy_Project.Helpers;

namespace pharmcy_Project.Controllers
{
  
    public class InvoicesController : APIBaseController
    {
        private readonly IGenericRepository<Invoice> _repo;
        private readonly IMapper _mapper;
        private readonly PharmcyContext _context;

        public InvoicesController(IGenericRepository<Invoice> Repo,IMapper mapper,PharmcyContext context )
        {
            _repo = Repo;
            _mapper = mapper;
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? customerId)
        {
            var query = _context.invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .ThenInclude(ii => ii.Medicine)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(i => i.IssueDate >= fromDate);

            if (toDate.HasValue)
                query = query.Where(i => i.IssueDate<= toDate);

            if (customerId.HasValue)
                query = query.Where(i => i.CustomerId == customerId);

            var invoices = await query.ToListAsync();
            var invoiceDtos = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            return Ok(invoiceDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoces = _context.invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .ThenInclude(i => i.Medicine)
                .FirstOrDefault(i => i.Id == id);
            if (invoces == null)
            {
                return NotFound();
            }
            var mapped=_mapper.Map<InvoiceDto>(invoces);
            return Ok(mapped);
               
                
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto dto)
        {
            // Validate customer exists
            var customer = await _context.customers.FindAsync(dto.CustomerId);
            if (customer == null) return BadRequest("Customer not found");

            var invoice = new Invoice
            {
                CustomerId = dto.CustomerId,
                IssueDate = DateTime.Now,
                Items = new List<InvoiceItem>()
            };

            foreach (var item in dto.Items)
            {
                var medicine = await _context.medicines.FindAsync(item.MedicineId);
                if (medicine == null) return BadRequest($"Medicine with ID {item.MedicineId} not found");

                if (medicine.StockQuantity < item.Quantity)
                    return BadRequest($"Not enough stock for medicine {medicine.Name}");

                invoice.Items.Add(new InvoiceItem
                {
                    MedicineId = item.MedicineId,
                    Quantity = item.Quantity,
                    UnitPrice = medicine.Price
                });

                // Update stock
                medicine.StockQuantity -= item.Quantity;
                _context.medicines.Update(medicine);
            }

            // Calculate total
            invoice.TotalAmount = invoice.Items.Sum(i => i.Quantity * i.UnitPrice);

            await _repo.AddAsync(invoice);
             _context.SaveChanges();

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoiceDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _repo.GetByIdAsync(id);
            if (invoice == null) return NotFound();

            _repo.Delete(invoice);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpGet("{id}/print")]
        public async Task<IActionResult> PrintInvoice(int id)
        {
            var invoice = await _context.invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .ThenInclude(ii => ii.Medicine)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null) return NotFound();

            // Generate PDF (using a library like DinkToPdf)
            var pdfGenerator = new PdfGenerator();
            var pdfBytes = pdfGenerator.GenerateInvoicePdf(invoice);

            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Id}.pdf");
        }
    }


}

