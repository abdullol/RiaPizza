using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiaPizza.Models;
using RiaPizza.Services.ThemeCustomization;

namespace RiaPizza.Controllers
{
    public class CustomizeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICustomizeThemeService _customizeThemeService;
        public CustomizeController(IWebHostEnvironment hostingEnvironment, ICustomizeThemeService customizeThemeService)
        {
            _customizeThemeService = customizeThemeService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var theme = await _customizeThemeService.GetThemeElements();
            return View(theme);
        }

        [HttpPost]
        public async Task<IActionResult> LogoUpdate(IFormFile file, string origin)
        {
            string UniqueFilename;
            if (file != null)
            {
                string UploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "CustomTheme");

                if (!Directory.Exists(UploadFolder))
                {
                    Directory.CreateDirectory(UploadFolder);
                }

                UniqueFilename = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(UploadFolder, UniqueFilename);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                var logo = origin + "/CustomTheme/" + UniqueFilename;
                await _customizeThemeService.UpdateLogo(logo);
            }
            return RedirectToAction("Index");
        }
    }
}