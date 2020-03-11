using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.DTOs.Auth;
using RiaPizza.DTOs.Role;

namespace RiaPizza.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.username, loginDto.password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Result = result.ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> LoginUser(IFormCollection form)
        {
            var loginDto = JsonConvert.DeserializeObject<LoginDto>(form["loginDto"]);
            var result = await _signInManager.PasswordSignInAsync(loginDto.username, loginDto.password, false, false);
            if (result.Succeeded)
                return Json("Success");

            return Json("Failed");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Rolelist =new SelectList( _roleManager.Roles,"Name","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                AppUser user = await _userManager.FindByNameAsync(registerDto.userName);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = registerDto.userName,
                        Email = registerDto.email,
                        FirstName = registerDto.firstName,
                        LastName = registerDto.lastName,
                        PhoneNumber = registerDto.phoneNumber,
                        City = registerDto.city,
                        Address = registerDto.address
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, registerDto.password);
                    
                    ViewBag.Message = "User successfully created!";
                    if (result.Succeeded) 
                    {
                        await  _userManager.AddToRoleAsync(user, registerDto.RoleName);
                        return RedirectToAction("AllUsers","Auth");    
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RegisterUser(IFormCollection form)
        {
            try
            {
                var registerDto = JsonConvert.DeserializeObject<RegisterDto>(form["registerDto"]);
                registerDto.userName = registerDto.email;
                AppUser user = await _userManager.FindByNameAsync(registerDto.userName);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = registerDto.userName,
                        Email = registerDto.email,
                        FirstName = registerDto.firstName,
                        LastName = registerDto.lastName,
                        PhoneNumber = registerDto.phoneNumber,
                        City = registerDto.city,
                        Address = registerDto.address
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, registerDto.password);
                    
                    return result.Succeeded ? Json("Success") : Json("Failed");
                }
                else
                    return Json("UsernameExists");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return Json("Failed");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                AppRole role = new AppRole { Name = roleDto.RoleName };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles", "Auth");
                }
            }

            return View(roleDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Roles() 
        {
            RoleDto roleList = new RoleDto();
            roleList.RoleList = _roleManager.Roles;
            return View(roleList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() 
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto) 
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) 
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user,
                    changePasswordDto.CurrentPassword,changePasswordDto.NewPassword);

                if (!result.Succeeded) 
                {
                    foreach (var error in result.Errors) 
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        ViewBag.Error = error.Description;
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(changePasswordDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePasswordUser(ChangePasswordDto changePasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var result = await _userManager.ChangePasswordAsync(user,
                    changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return RedirectToAction("ChangePassword", "MyAccount");
                }
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("PersonalData", "MyAccount");
            }
            else
            {
                return RedirectToAction("ChangePassword", "MyAccount");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AllUsers() 
        {
            var users =await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
            return View(users);
        }

        public JsonResult GetLoggedInUser()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                var user = _userManager.FindByIdAsync(userId).Result;
                return Json(user);
            }
            else
                return Json(false);
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyAccount()
        {
            return View();
        }
    }
}