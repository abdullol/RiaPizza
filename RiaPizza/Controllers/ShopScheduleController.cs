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

        public async Task<IActionResult> Index()
        {
            var schedule = await _scheduleService.GetSchedule();
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
            var schedule = await _scheduleService.GetSchedule();
            return Json(schedule);
        }

    }
}