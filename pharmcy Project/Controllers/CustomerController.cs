using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmcy.Repository.Data;
using Pharmcy.Core.Entities;
using Pharmcy.Core.Repositories;
using pharmcy_Project.DTO;

namespace pharmcy_Project.Controllers
{

    public class CustomerController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly PharmcyContext _context;
        private readonly IGenericRepository<Customer> _repo;

        public CustomerController(IMapper mapper, PharmcyContext context, IGenericRepository<Customer> Repo)
        {
            _mapper = mapper;
            _context = context;
            _repo = Repo;
        }
        [HttpGet]
        public async Task<ActionResult<Customer>> GetAllCustomers()
        {
            var customers = await _repo.GetAllAsync();
            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDto);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerByid(int id)
        {
            var Customer = await _repo.GetByIdAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }
            var cutomerDto = _mapper.Map<CustomerDto>(Customer);
            return Ok(cutomerDto);
        }


        [HttpPost]
        public async Task<IActionResult> Crete([FromBody] CreateUpdateCustomerDto dto)
        {
            var Customer = _mapper.Map<Customer>(dto);
            await _repo.AddAsync(Customer);
            _context.SaveChanges();
            var customerDto = _mapper.Map<CustomerDto>(Customer);
            return CreatedAtAction(nameof(GetCustomerByid), new { id = Customer.Id }, customerDto);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CreateUpdateCustomerDto dto,int id)
        {
            var customer=await _repo.GetByIdAsync(id);
            if (customer ==null)
            {
                return NotFound();
            }
            _mapper.Map(dto, customer);
            _repo.Update(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/purchases")]
        public async Task<IActionResult> GetCustomerPurchases(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer == null) return NotFound();

            // Assuming you have navigation property Customer.Invoices
            var purchases = await _context.invoices
                .Where(i => i.CustomerId == id)
                .Include(i => i.Items)
                .ThenInclude(ii => ii.Medicine)
                .ToListAsync();

            var purchaseDtos = _mapper.Map<IEnumerable<InvoiceDto>>(purchases);
            return Ok(purchaseDtos);
        }
    }
}
