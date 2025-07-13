using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmcy.Repository.Data;
using Pharmcy.Core.Entities;
using pharmcy_Project.DTO;

namespace pharmcy_Project.Controllers
{
    
    public class ReportsController : APIBaseController
    {
        private readonly PharmcyContext _context;

        public ReportsController(PharmcyContext context)
        {
            _context = context;
        }

        // GET: api/reports/sales
        [HttpGet("sales")]
            public async Task<IActionResult> GetSalesReport(
                [FromQuery] string period = "daily") // daily, weekly, monthly
            {
                var now = DateTime.Now;
                IQueryable<Invoice> query = _context.invoices;

                switch (period.ToLower())
                {
                    case "weekly":
                        var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
                        query = query.Where(i => i.IssueDate >= startOfWeek);
                        break;
                    case "monthly":
                        var startOfMonth = new DateTime(now.Year, now.Month, 1);
                        query = query.Where(i => i.IssueDate >= startOfMonth);
                        break;
                    default: // daily
                        query = query.Where(i => i.IssueDate.Date == now.Date);
                        break;
                }

                var salesData = await query
                    .GroupBy(i => i.IssueDate.Date)
                    .Select(g => new {
                        Date = g.Key,
                        TotalSales = g.Sum(i => i.TotalAmount),
                        Count = g.Count()
                    }).ToListAsync();
                    

                return Ok(salesData);
            }

            // GET: api/reports/top-medicines
            [HttpGet("top-medicines")]
            public async Task<IActionResult> GetTopMedicines(
                [FromQuery] int limit = 5,
                [FromQuery] DateTime? fromDate = null,
                [FromQuery] DateTime? toDate = null)
            {
                var query = _context.invoiceItems
                    .Include(ii => ii.Medicine)
                    .AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(ii => ii.Invoice.IssueDate >= fromDate);

                if (toDate.HasValue)
                    query = query.Where(ii => ii.Invoice.IssueDate <= toDate);

                var topMedicines = await query
                    .GroupBy(ii => new { ii.MedicineId, ii.Medicine.Name })
                    .OrderByDescending(g => g.Sum(ii => ii.Quantity))
                    .Take(limit)
                    .Select(g => new {
                        MedicineId = g.Key.MedicineId,
                        MedicineName = g.Key.Name,
                        TotalSold = g.Sum(ii => ii.Quantity),
                        TotalRevenue = g.Sum(ii => ii.Quantity * ii.UnitPrice)
                    })
                    .ToListAsync();

                return Ok(topMedicines);
            }

        // GET: api/reports/revenue
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport(
            [FromQuery] DateTime fromDate,
                [FromQuery] DateTime toDate)
        {
            var revenueData = await _context.invoices
                .Where(i => i.IssueDate >= fromDate && i.IssueDate <= toDate)
                .GroupBy(i => 1) // تجميع كل الفواتير معاً
                .Select(g => new RevenueReportDto
                {
                    TotalRevenue = g.Sum(i => i.TotalAmount),
                    AverageSale = g.Average(i => i.TotalAmount),
                    Count = g.Count()
                })
                .FirstOrDefaultAsync();

            return Ok(revenueData ?? new RevenueReportDto());
        }
    }
        }
    