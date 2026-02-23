using LuxurySalon.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxurySalon.Domain.Entities
{
    public class Service : BaseEntity
    {
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int DurationInMinutes { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<StylistService> StylistServices { get; set; } = new HashSet<StylistService>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
