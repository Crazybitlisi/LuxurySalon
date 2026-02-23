using LuxurySalon.Application.DTOs;
using LuxurySalon.Application.Interfaces;
using LuxurySalon.Domain.Entities;
using LuxurySalon.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxurySalon.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IApplicationDbContext _context;

        public AppointmentService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SlotDto>> GetAvailableSlotsAsync(int stylistId, int serviceId, DateTime date)
        {
            var stylist = await _context.Stylists.FindAsync(stylistId);
            var service = await _context.Services.FindAsync(serviceId);

            if (stylist == null || service == null) return Enumerable.Empty<SlotDto>();

            // 1. Get existing appointments for the stylist on that day
            var existingAppointments = await _context.Appointments
                .Where(a => a.StylistId == stylistId && a.StartTime.Date == date.Date && a.Status != AppointmentStatus.Cancelled)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            // 2. Define working boundaries
            var workStart = date.Date.Add(stylist.WorkStartTime);
            var workEnd = date.Date.Add(stylist.WorkEndTime);

            // 3. Generate slots
            var slots = new List<SlotDto>();
            var currentPos = workStart;

            while (currentPos.AddMinutes(service.DurationInMinutes) <= workEnd)
            {
                var slotEnd = currentPos.AddMinutes(service.DurationInMinutes);
                
                // Check if slot overlaps with any existing appointment
                bool isOverlapping = existingAppointments.Any(a => 
                    (currentPos >= a.StartTime && currentPos < a.EndTime) || 
                    (slotEnd > a.StartTime && slotEnd <= a.EndTime) ||
                    (a.StartTime >= currentPos && a.StartTime < slotEnd));

                // No booking in the past
                bool isPast = currentPos < DateTime.UtcNow;

                slots.Add(new SlotDto
                {
                    StartTime = currentPos,
                    EndTime = slotEnd,
                    IsAvailable = !isOverlapping && !isPast
                });

                // Move forward by 30 minutes for slot selection flexibility
                currentPos = currentPos.AddMinutes(30); 
            }

            return slots;
        }

        public async Task<bool> CreateAppointmentAsync(string customerId, CreateAppointmentRequest request)
        {
            var service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null) return false;

            var endTime = request.StartTime.AddMinutes(service.DurationInMinutes);

            // Start Transaction for atomic check and reserve
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Double check for overlap (Concurrency & Double Booking Protection)
                bool hasConflict = await _context.Appointments
                    .AnyAsync(a => a.StylistId == request.StylistId && 
                                   a.Status != AppointmentStatus.Cancelled &&
                                   ((request.StartTime >= a.StartTime && request.StartTime < a.EndTime) || 
                                    (endTime > a.StartTime && endTime <= a.EndTime)));

                if (hasConflict) return false;

                var appointment = new Appointment
                {
                    CustomerId = customerId,
                    GuestIdentifier = request.GuestIdentifier,
                    StylistId = request.StylistId,
                    ServiceId = request.ServiceId,
                    StartTime = request.StartTime,
                    EndTime = endTime,
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync(); // Optimistic concurrency check happens here via RowVersion

                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                return false; // Concurrency conflict detected
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
