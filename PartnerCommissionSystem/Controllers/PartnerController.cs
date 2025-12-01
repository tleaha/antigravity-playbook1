using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerCommissionSystem.Data;
using PartnerCommissionSystem.Models;

namespace PartnerCommissionSystem.Controllers
{
    [Authorize(Roles = "Partner")]
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReferralCodes()
        {
            var partnerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var codes = await _context.ReferralCodes.Where(r => r.PartnerId == partnerId).ToListAsync();
            return View(codes);
        }

        [HttpGet]
        public IActionResult CreateReferralCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferralCode(ReferralCode model)
        {
            var partnerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            
            // Basic validation: check if code exists globally
            if (await _context.ReferralCodes.AnyAsync(r => r.Code == model.Code))
            {
                ModelState.AddModelError("Code", "This code is already taken.");
                return View(model);
            }

            model.PartnerId = partnerId;
            _context.ReferralCodes.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReferralCodes));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReferralCode(int id)
        {
            var partnerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var code = await _context.ReferralCodes.FirstOrDefaultAsync(r => r.Id == id && r.PartnerId == partnerId);
            if (code != null)
            {
                _context.ReferralCodes.Remove(code);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ReferralCodes));
        }

        public async Task<IActionResult> Commissions()
        {
            var partnerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var commissions = await _context.Commissions
                .Include(c => c.Booking)
                .ThenInclude(b => b.Accommodation)
                .Where(c => c.PartnerId == partnerId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(commissions);
        }
    }
}
