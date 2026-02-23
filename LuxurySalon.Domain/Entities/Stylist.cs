using LuxurySalon.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxurySalon.Domain.Entities
{
    public class Stylist : BaseEntity
    {
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Biyografi")]
        public string? Bio { get; set; }

        [Display(Name = "Profil Resmi")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Mesai Başlangıç")]
        public TimeSpan WorkStartTime { get; set; } = new TimeSpan(9, 0, 0); // 09:00

        [Display(Name = "Mesai Bitiş")]
        public TimeSpan WorkEndTime { get; set; } = new TimeSpan(18, 0, 0); // 18:00

        [Display(Name = "Tam Ad")]
        public string FullName => $"{FirstName} {LastName}";

        public virtual ICollection<StylistService> StylistServices { get; set; } = new HashSet<StylistService>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
