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

        public ShopScheduleController(IScheduleService service)
        {
            _scheduleService = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdate()
        {
            ShopSchedule schedule = JsonConvert.DeserializeObject<ShopSchedule>(Request.Form["shopSchedule"]);
            await _scheduleService.AddorUpdate(schedule);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> GetSchedule()
        {
            var schedule = await _scheduleService.GetSchedule();
            return Json(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleShop(string status)
        {
            await _scheduleService.ToggleShop(status);
            return RedirectToAction("Index","Dashboard");
        }

        public async Task<JsonResult> GetShopStatus()
        {
            var schedule = await _scheduleService.GetShopStatus();
            return Json(schedule);
        }

    }
}