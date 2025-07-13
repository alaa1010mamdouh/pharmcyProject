using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmcy.Repository.Data;
using Pharmcy.Core.Entities;
using Pharmcy.Core.Repositories;
using pharmcy_Project.DTO;
using pharmcy_Project.Errors;

namespace pharmcy_Project.Controllers
{
 
    public class MedicineController : APIBaseController
    {
        private readonly IGenericRepository<Medicine> _medicineRepo;
        private readonly PharmcyContext _context;
        private readonly IMapper _mapper;

        public MedicineController( IGenericRepository<Medicine> medicineRepo,PharmcyContext context,IMapper mapper)
        {
            _medicineRepo = medicineRepo;
            _context = context;
            _mapper = mapper;
        }
        //Get All Medicines
        [Authorize (AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string ?searchTerm, [FromQuery] string ?category)
        {
            try
            {
                var query = _context.medicines.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                    query = query.Where(m => m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm));

                if (!string.IsNullOrEmpty(category))
                    query = query.Where(m => m.Category == category);

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Get By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicineById(int id)
        {
            var medicine = await _medicineRepo.GetByIdAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            var mapped=_mapper.Map<Medicine,MedicineDto>(medicine);
            return Ok(mapped);
        }
        //Delete Medicine
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Medicine),200)]
        [ProducesResponseType(typeof(ApiResponse),404)]
        public async Task<ActionResult<Medicine>> DeleteMedicine(int id)
        {
            var medicine = await _medicineRepo.GetByIdAsync(id);
            if (medicine == null)
            {
                return NotFound(new ApiResponse(400));
            }
            _medicineRepo.Delete(medicine);
           _context.SaveChanges();
            return NoContent();
        }

        //Add Medicine
        [HttpPost]
        public async Task<ActionResult<Medicine>> AddMedicine([FromBody]Medicine medicine)
        {
            if (medicine == null)
            {
                return BadRequest("Medicine cannot be null");
            }
            await _medicineRepo.AddAsync(medicine);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMedicineById), new { id = medicine.Id }, medicine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Medicine medicine)
        {
            if (id != medicine.Id) return BadRequest();
            _medicineRepo.Update(medicine);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpGet("expiry-alerts")]
        public async Task<IActionResult> GetExpiryAlerts([FromQuery] int days = 30)
        {
            var dateThreshold = DateTime.Now.AddDays(days);
            var medicines = await _medicineRepo.FindAsync(m => m.ExpiryDate <= dateThreshold);
            return Ok(medicines);
        }

        // GET: api/medicines/low-stock
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
        {
            var medicines = await _medicineRepo.FindAsync(m => m.StockQuantity <= threshold);
            return Ok(medicines);
        }
    }
}
