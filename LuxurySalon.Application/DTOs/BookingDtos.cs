using System;

namespace LuxurySalon.Application.DTOs
{
    public class SlotDto
    {
    akıllı ol pezevenk 
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
