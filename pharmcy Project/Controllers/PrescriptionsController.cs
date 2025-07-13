using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmcy.Repository.Data;
using Pharmcy.Core.Entities;
using Pharmcy.Core.Repositories;
using pharmcy_Project.DTO;
using System.Security.Claims;

namespace pharmcy_Project.Controllers
{
   
    public class PrescriptionsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly PharmcyContext _context;
        private readonly IGenericRepository<Prescription> _repo;
        public PrescriptionsController(IMapper mapper, PharmcyContext context, IGenericRepository<Prescription> Repo)
        {
            _mapper = mapper;
            _context = context;
            _repo = Repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int prescriptionId, [FromBody] CreatePrescriptionItemDto dto)
        {
            var prescription = await _context.prescriptions
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.Id == prescriptionId);

            if (prescription == null)
                return NotFound("Prescription not found");

            var medicine = await _context.medicines.FindAsync(dto.MedicineId);
            if (medicine == null)
                return BadRequest("Medicine not found");

            var item = _mapper.Map<PrescriptionItem>(dto);
            item.PrescriptionId = prescriptionId;

            prescription.PrescriptionItems.Add(item);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<PrescriptionItemDto>(item);
            return CreatedAtAction(
                nameof(GetItem),
                new { prescriptionId, id = item.Id },
                result);
        }

        // GET: api/prescriptions/5/items/3
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int prescriptionId, int id)
        {
            var item = await _context.prescriptionItems
                .Include(pi => pi.Medicine)
                .Include(pi => pi.Prescription)
                .FirstOrDefaultAsync(pi => pi.Id == id && pi.PrescriptionId == prescriptionId);

            if (item == null)
                return NotFound();

            var result = _mapper.Map<PrescriptionItemDto>(item);
            return Ok(result);
        }

        // DELETE: api/prescriptions/5/items/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveItem(int prescriptionId, int id)
        {
            var item = await _context.prescriptionItems
                .FirstOrDefaultAsync(pi => pi.Id == id && pi.PrescriptionId == prescriptionId);

            if (item == null)
                return NotFound();

            _context.prescriptionItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

      



    }
}

