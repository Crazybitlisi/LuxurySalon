using LuxurySalon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxurySalon.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new LuxurySalonDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<LuxurySalonDbContext>>());

            // Check if database exists and apply migrations
            context.Database.Migrate();

            // Seed Roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUsername = "admin";
            var adminUser = await userManager.FindByNameAsync(adminUsername);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = "admin@luxurysalon.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                // Ensure password is reset to 1234 if user already exists
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, "1234");
            }

            // Seed Services
            var servicesList = new List<Service>
            {
                new Service { Name = "Özel Kesim & Stil", Description = "Yüz şeklinize özel, modern ve klasik kesim teknikleriyle stilinizi yeniden tanımlayın.", Price = 350.00m, DurationInMinutes = 60 },
                new Service { Name = "Balayaj Sanatı", Description = "Doğal ışıltılar ve geçişlerle saçınıza derinlik katan sanatsal renklendirme.", Price = 1200.00m, DurationInMinutes = 120 },
                new Service { Name = "Lüks Saç Bakımı", Description = "Premium maskeler ve masaj eşliğinde saçınıza hak ettiği nemi ve canlılığı geri kazandırın.", Price = 450.00m, DurationInMinutes = 45 },
                new Service { Name = "Tasarım Fön", Description = "Özel günleriniz veya günlük şıklığınız için kusursuz hacim ve parlaklık.", Price = 200.00m, DurationInMinutes = 30 }
            };

            if (!context.Services.Any())
            {
                context.Services.AddRange(servicesList);
                context.SaveChanges();
            }
            else
            {
                // Force update English names if they exist
                var existingServices = context.Services.ToList();
                foreach (var s in existingServices)
                {
                    if (s.Name == "Signature Haircut") { s.Name = "Özel Kesim & Stil"; s.Description = "Yüz şeklinize özel, modern ve klasik kesim teknikleriyle stilinizi yeniden tanımlayın."; s.Price = 350.00m; }
                    else if (s.Name == "Balayage Artistry") { s.Name = "Balayaj Sanatı"; s.Description = "Doğal ışıltılar ve geçişlerle saçınıza derinlik katan sanatsal renklendirme."; s.Price = 1200.00m; }
                    else if (s.Name == "Luxury Scalp Treatment") { s.Name = "Lüks Saç Bakımı"; s.Description = "Premium maskeler ve masaj eşliğinde saçınıza hak ettiği nemi ve canlılığı geri kazandırın."; s.Price = 450.00m; }
                    else if (s.Name == "Designer Blowout") { s.Name = "Tasarım Fön"; s.Description = "Özel günleriniz veya günlük şıklığınız için kusursuz hacim ve parlaklık."; s.Price = 200.00m; }
                }
                context.SaveChanges();
            }

            // Seed Stylists
            var stylistsList = new List<Stylist>
            {
                new Stylist { FirstName = "Alexander", LastName = "Vogue", Bio = "Sektörde 15 yıllık tecrübesiyle, haute couture kesim ve modern stil uzmanı.", ImageUrl = "stylist1.jpg", WorkStartTime = new TimeSpan(9, 0, 0), WorkEndTime = new TimeSpan(18, 0, 0) },
                new Stylist { FirstName = "Elena", LastName = "Gold", Bio = "Renk kimyası ve sofistike balayaj teknikleri konusunda uzman baş stilist.", ImageUrl = "stylist2.jpg", WorkStartTime = new TimeSpan(10, 0, 0), WorkEndTime = new TimeSpan(19, 0, 0) },
                new Stylist { FirstName = "Julian", LastName = "Sharp", Bio = "Hassas kesim teknikleri ve geleneksel erkek bakım ritüelleri ustası.", ImageUrl = "stylist3.jpg", WorkStartTime = new TimeSpan(8, 0, 0), WorkEndTime = new TimeSpan(17, 0, 0) }
            };

            if (!context.Stylists.Any())
            {
                context.Stylists.AddRange(stylistsList);
                context.SaveChanges();
            }
            else
            {
                var existingStylists = context.Stylists.ToList();
                foreach (var s in existingStylists)
                {
                    if (s.FirstName == "Alexander" && s.Bio.Contains("Master stylist")) s.Bio = "Sektörde 15 yıllık tecrübesiyle, haute couture kesim ve modern stil uzmanı.";
                    if (s.FirstName == "Elena" && s.Bio.Contains("Specialist")) s.Bio = "Renk kimyası ve sofistike balayaj teknikleri konusunda uzman baş stilist.";
                    if (s.FirstName == "Julian" && s.Bio.Contains("Expert")) s.Bio = "Hassas kesim teknikleri ve geleneksel erkek bakım ritüelleri ustası.";
                }
                context.SaveChanges();
            }

            // Ensure mappings exist
            var allS = context.Services.ToList();
            var allStylists = context.Stylists.ToList();
            foreach (var stylist in allStylists)
            {
                foreach (var service in allS)
                {
                    if (!context.StylistServices.Any(ss => ss.StylistId == stylist.Id && ss.ServiceId == service.Id))
                    {
                        context.StylistServices.Add(new StylistService { StylistId = stylist.Id, ServiceId = service.Id });
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
