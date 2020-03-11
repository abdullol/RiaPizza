using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.DTOs.Auth;
using RiaPizza.DTOs.User;
using RiaPizza.Services.AccountService;

namespace RiaPizza.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly IAccountService _service;
        private readonly UserManager<AppUser> _userManager;
        public MyAccountController(IAccountService service, UserManager<AppUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        public IActionResult Orders()
        {
            return View();
        }
        public async Task<IActionResult> PersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            var updateProfile = new UpdateProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(updateProfile);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalData(UpdateProfileDto updateProfile)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            user.FirstName = updateProfile.FirstName;
            user.LastName = updateProfile.LastName;
            user.PhoneNumber = updateProfile.PhoneNumber;
            user.Email = updateProfile.Email;
            user.UserName = updateProfile.Email;

            await _userManager.UpdateAsync(user);
            return View();
        }
        public async Task<IActionResult> Addresses()
        {
            var userId = Int32.Parse(GetUserId());
            var addresses = await _service.GetAddresses(userId);
            return View(addresses);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        private string GetUserId()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            return userId;
        }
        public async Task<JsonResult> GetUserAddresses(int id)
        {
            var addresses = await _service.GetAddresses(id);
            return Json(addresses);
        }
    }
}