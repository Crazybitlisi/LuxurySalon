using LuxurySalon.Application.Interfaces;
using LuxurySalon.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxurySalon.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IApplicationDbContext _context;

        public AdminDashboardService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardStatsDto> GetDashboardStatsAsync()
        {
            return new AdminDashboardStatsDto
            {
                TotalServices = await _context.Services.CountAsync(s => s.IsActive),
                TotalStylists = await _context.Stylists.CountAsync(s => s.IsActive),
                PendingAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending),
                ConfirmedAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Confirmed),
                TotalRevenue = await _context.Appointments
                    .Where(a => a.Status == AppointmentStatus.Completed)
                    .SumAsync(a => a.Service.Price)
            };
        }

        public async Task<IEnumerable<AdminAppointmentDto>> GetRecentAppointmentsAsync(int count)
        {
            return await _context.Appointments
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .Select(a => new AdminAppointmentDto
                {
                    Id = a.Id,
                    CustomerName = a.Customer != null ? a.Customer.FirstName + " " + a.Customer.LastName : "Misafir",
                    StylistName = a.Stylist.FirstName + " " + a.Stylist.LastName,
                    ServiceName = a.Service.Name,
                    StartTime = a.StartTime,
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }
    }
}
