using LuxurySalon.Application.DTOs;
using LuxurySalon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LuxurySalon.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IApplicationDbContext _context;

        public BookingController(IAppointmentService appointmentService, IApplicationDbContext context)
        {
            _appointmentService = appointmentService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.Where(s => s.IsActive).ToListAsync();
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> GetStylists(int serviceId)
        {
            var stylistsData = await _context.Stylists
                .Where(s => s.IsActive && s.StylistServices.Any(ss => ss.ServiceId == serviceId))
                .ToListAsync();

            var result = stylistsData.Select(s => new {
                s.Id,
                s.FullName,
                s.ImageUrl,
                s.Bio,
                WorkStartTime = s.WorkStartTime.ToString(@"hh\:mm"),
                WorkEndTime = s.WorkEndTime.ToString(@"hh\:mm")
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> AvailableSlots(int stylistId, int serviceId, DateTime date)
        {
            if (date == default) date = DateTime.Today;
            if (date < DateTime.Today) return BadRequest("Geçmiş bir tarihe randevu alınamaz.");
            
            var slots = await _appointmentService.GetAvailableSlotsAsync(stylistId, serviceId, date);
            return Json(slots);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([FromBody] CreateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // If not logged in, handle Guest Identifier via Cookie
            if (string.IsNullOrEmpty(userId))
            {
                var guestId = Request.Cookies["GuestId"];
                if (string.IsNullOrEmpty(guestId))
                {
                    guestId = Guid.NewGuid().ToString();
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(10),
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Strict
                    };
                    Response.Cookies.Append("GuestId", guestId, cookieOptions);
                }
                request.GuestIdentifier = guestId;
            }

            var result = await _appointmentService.CreateAppointmentAsync(userId, request);

            if (result)
            {
                return Ok(new { Message = "Randevunuz başarıyla oluşturuldu." });
            }

            return Conflict(new { Message = "Seçilen saat dilimi artık uygun değil. Lütfen başka bir zaman seçin." });
        }
    }
}
