using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}