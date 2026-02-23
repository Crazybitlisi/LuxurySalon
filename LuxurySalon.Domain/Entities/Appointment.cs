using LuxurySalon.Domain.Common;
using LuxurySalon.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LuxurySalon.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        [Display(Name = "Müşteri Kimliği")]
        public string? CustomerId { get; set; }
        [Display(Name = "Müşteri")]
        public virtual ApplicationUser? Customer { get; set; }

        [Display(Name = "Uzman Kimliği")]
        public int StylistId { get; set; }
        [Display(Name = "Uzman")]
        public virtual Stylist Stylist { get; set; } = null!;

        [Display(Name = "Hizmet Kimliği")]
        public int ServiceId { get; set; }
        [Display(Name = "Hizmet")]
        public virtual Service Service { get; set; } = null!;

        [Display(Name = "Başlangıç Zamanı")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Bitiş Zamanı")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Durum")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [Display(Name = "Misafir Kimliği")]
        public string? GuestIdentifier { get; set; }

        [Display(Name = "Notlar")]
        public string? CustomerNotes { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
