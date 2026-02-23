using FluentValidation;
using LuxurySalon.Application.Interfaces;
using LuxurySalon.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LuxurySalon.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            
            return services;
        }
    }
}
