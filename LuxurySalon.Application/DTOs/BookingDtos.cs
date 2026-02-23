using System;

namespace LuxurySalon.Application.DTOs
{
    public class SlotDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class CreateAppointmentRequest
    {
        public int ServiceId { get; set; }
        public int StylistId { get; set; }
        public DateTime StartTime { get; set; }
        public string? CustomerNotes { get; set; }
        public string? GuestIdentifier { get; set; }
    }
}
