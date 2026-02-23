using LuxurySalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace LuxurySalon.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Service> Services { get; }
        DbSet<Stylist> Stylists { get; }
        DbSet<StylistService> StylistServices { get; }
        DbSet<Appointment> Appointments { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
