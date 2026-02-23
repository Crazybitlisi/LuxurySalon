using LuxurySalon.Application.Interfaces;
using LuxurySalon.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LuxurySalon.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly IApplicationDbContext _context;

        public AdminController(IAdminDashboardService dashboardService, IApplicationDbContext context)
        {
            _dashboardService = dashboardService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = await _dashboardService.GetDashboardStatsAsync();
            var recentAppointments = await _dashboardService.GetRecentAppointmentsAsync(10);
            
            ViewBag.Stats = stats;
            return View(recentAppointments);
        }

        public async Task<IActionResult> Appointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.Service)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }
    }
}
