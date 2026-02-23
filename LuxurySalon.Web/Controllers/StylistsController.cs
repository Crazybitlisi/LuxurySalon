using LuxurySalon.Application.Interfaces;
using LuxurySalon.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LuxurySalon.Web.Controllers
{
    public class StylistsController : Controller
    {
        private readonly IApplicationDbContext _context;

        public StylistsController(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Stylists.Where(s => s.IsActive).ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stylist stylist)
        {
            if (ModelState.IsValid)
            {
                _context.Stylists.Add(stylist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stylist);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var stylist = await _context.Stylists.FindAsync(id);
            if (stylist == null) return NotFound();
            return View(stylist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Stylist stylist)
        {
            if (id != stylist.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Stylists.Update(stylist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StylistExists(stylist.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stylist);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var stylist = await _context.Stylists.FindAsync(id);
            if (stylist == null) return NotFound();

            stylist.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StylistExists(int id)
        {
            return _context.Stylists.Any(e => e.Id == id);
        }
    }
}
