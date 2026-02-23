using LuxurySalon.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuxurySalon.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<SlotDto>> GetAvailableSlotsAsync(int stylistId, int serviceId, DateTime date);
        Task<bool> CreateAppointmentAsync(string customerId, CreateAppointmentRequest request);
    }
}
