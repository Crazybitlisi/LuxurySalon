namespace LuxurySalon.Domain.Entities
{
    public class StylistService
    {
        public int StylistId { get; set; }
        public virtual Stylist Stylist { get; set; } = null!;

        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;
    }
}
