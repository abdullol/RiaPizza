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

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
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
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Register()
        {
            ViewBag.Rolelist = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
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
                        await _userManager.AddToRoleAsync(user, registerDto.RoleName);
                        return RedirectToAction("AllUsers", "Auth");
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
                    changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

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

        public async Task<ActionResult> AllUsers()
        {
            ViewBag.Role = new SelectList(_roleManager.Roles, "Name", "Name");
            var users = await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
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

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            string userId = Id.ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id =  { userId } can not be found";
                return View("Not Found");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UpdateUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            //getting roles in dropdown
            ViewBag.Role = _roleManager.Roles.ToList();
            ViewBag.UserRole = _userManager.GetRolesAsync(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserDto usermodel)
        {
            string userId = usermodel.Id.ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id =  { userId } can not be found";
                return View("Not Found");
            }
            else
            {
                user.FirstName = usermodel.FirstName;
                user.LastName = usermodel.LastName;
                user.UserName = usermodel.UserName;
                user.PhoneNumber = usermodel.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    RedirectToAction("AllUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("AllUsers");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            string userId = Id.ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id =  { userId } can not be found";
                return View("Not Found");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    RedirectToAction("AllUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("AllUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(int id)
        {
            ViewBag.userId = id;
            var userid = id.ToString();
            var user = await _userManager.FindByIdAsync(userid);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("Not Found");
            }

            var model = new List<UseRoleDto>();

            foreach (var role in _roleManager.Roles)
            {
                var userRoleDto = new UseRoleDto
                {
                    RoleId = role.Id.ToString(),
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleDto.IsSelected = true;
                }
                else
                {
                    userRoleDto.IsSelected = false;
                }

                model.Add(userRoleDto);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(UseRoleDto model, int id)
        {
            var userId = id.ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Edit", new { Id = userId });
        }
    }
}