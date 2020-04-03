using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiaPizza.Models;
using RiaPizza.Services.ScheduleService;

namespace RiaPizza.Controllers
{
    public class ShopScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ShopScheduleController(IScheduleService service,
            IHostingEnvironment hostingEnvironment)
        {
            _scheduleService = service;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var schedule = _scheduleService.GetSchedule();
            ViewBag.isOpen = true;
            return View(schedule);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ShopSchedule timings, string schedule)
        {
            await _scheduleService.AddorUpdate(timings);
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSchedule()
        {
            await _scheduleService.DeleteSchedule();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleShop(string status)
        {
            await _scheduleService.ToggleShop(status);
            return RedirectToAction("Index","Dashboard");
        }

        public async Task<JsonResult> GetShopStatus()
        {
            var schedule = _scheduleService.GetSchedule();
            return Json(schedule);
        }

        public async Task<ContentResult> LogoUpdate()
        {
            var category = new ShopSchedule();
            var file = Request.Form.Files[0];
            string UniqueFilename;
            if (file != null)
            {
                string UploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

                if (!Directory.Exists(UploadFolder))
                {
                    Directory.CreateDirectory(UploadFolder);
                }

                UniqueFilename = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(UploadFolder, UniqueFilename);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                category.ShopLogo = UniqueFilename;
                await _scheduleService.AddorUpdate(category);
            }
            return Content("Success");
        }
    }
}