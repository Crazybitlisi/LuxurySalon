using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuxurySalon.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<AdminAppointmentDto>> GetRecentAppointmentsAsync(int count);
    }

    public class AdminDashboardStatsDto
    {
        public int TotalServices { get; set; }
        public int TotalStylists { get; set; }
        public int PendingAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class AdminAppointmentDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string StylistName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public System.DateTime StartTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
