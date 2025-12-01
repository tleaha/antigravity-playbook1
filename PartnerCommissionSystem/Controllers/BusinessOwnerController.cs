using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerCommissionSystem.Data;
using PartnerCommissionSystem.Models;

namespace PartnerCommissionSystem.Controllers
{
    [Authorize(Roles = "BusinessOwner")]
    public class BusinessOwnerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessOwnerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Accommodations()
        {
            var ownerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var accommodations = await _context.Accommodations.Where(a => a.BusinessOwnerId == ownerId).ToListAsync();
            return View(accommodations);
        }

        [HttpGet]
        public IActionResult CreateAccommodation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccommodation(Accommodation model)
        {
            var ownerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            
            // Validate Referral Code if provided
            if (!string.IsNullOrEmpty(model.ReferralCode))
            {
                var referralCode = await _context.ReferralCodes.FirstOrDefaultAsync(r => r.Code == model.ReferralCode);
                if (referralCode == null)
                {
                    ModelState.AddModelError("ReferralCode", "Invalid Referral Code.");
                    return View(model);
                }
            }

            model.BusinessOwnerId = ownerId;
            _context.Accommodations.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Accommodations));
        }

        [HttpPost]
        public async Task<IActionResult> Book(int accommodationId, decimal amount)
        {
            var accommodation = await _context.Accommodations.FindAsync(accommodationId);
            if (accommodation == null) return NotFound();

            var booking = new Booking
            {
                AccommodationId = accommodationId,
                Amount = amount,
                BookingDate = DateTime.Now
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Calculate Commission
            if (!string.IsNullOrEmpty(accommodation.ReferralCode))
            {
                var referralCode = await _context.ReferralCodes.Include(r => r.Partner).FirstOrDefaultAsync(r => r.Code == accommodation.ReferralCode);
                if (referralCode != null)
                {
                    var commissionAmount = amount * 0.10m; // 10% Commission
                    var commission = new Commission
                    {
                        Amount = commissionAmount,
                        BookingId = booking.Id,
                        PartnerId = referralCode.PartnerId,
                        CreatedAt = DateTime.Now
                    };
                    _context.Commissions.Add(commission);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Accommodations));
        }
    }
}
