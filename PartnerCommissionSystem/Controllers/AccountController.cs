using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerCommissionSystem.Data;
using PartnerCommissionSystem.Models;
using PartnerCommissionSystem.ViewModels;
using System.Security.Claims;

namespace PartnerCommissionSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == "Partner")
                {
                    var partner = await _context.Partners.FirstOrDefaultAsync(p => p.Username == model.Username && p.Password == model.Password);
                    if (partner != null)
                    {
                        await SignInUser(partner.Id.ToString(), partner.Name, "Partner");
                        return RedirectToAction("Index", "Partner");
                    }
                }
                else if (model.Role == "BusinessOwner")
                {
                    var owner = await _context.BusinessOwners.FirstOrDefaultAsync(b => b.Username == model.Username && b.Password == model.Password);
                    if (owner != null)
                    {
                        await SignInUser(owner.Id.ToString(), owner.Name, "BusinessOwner");
                        return RedirectToAction("Index", "BusinessOwner");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == "Partner")
                {
                    if (await _context.Partners.AnyAsync(p => p.Username == model.Username))
                    {
                        ModelState.AddModelError("", "Username already exists.");
                        return View(model);
                    }
                    var partner = new Partner { Username = model.Username, Password = model.Password, Name = model.Name };
                    _context.Partners.Add(partner);
                    await _context.SaveChangesAsync();
                    await SignInUser(partner.Id.ToString(), partner.Name, "Partner");
                    return RedirectToAction("Index", "Partner");
                }
                else if (model.Role == "BusinessOwner")
                {
                    if (await _context.BusinessOwners.AnyAsync(b => b.Username == model.Username))
                    {
                        ModelState.AddModelError("", "Username already exists.");
                        return View(model);
                    }
                    var owner = new BusinessOwner { Username = model.Username, Password = model.Password, Name = model.Name };
                    _context.BusinessOwners.Add(owner);
                    await _context.SaveChangesAsync();
                    await SignInUser(owner.Id.ToString(), owner.Name, "BusinessOwner");
                    return RedirectToAction("Index", "BusinessOwner");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(string userId, string name, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}
