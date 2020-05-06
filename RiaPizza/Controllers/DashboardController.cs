using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiaPizza.Models;
using RiaPizza.Services.OrderService;
using RiaPizza.Services.ScheduleService;

namespace RiaPizza.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IOrderService _orderService;
        private readonly IScheduleService _scheduleService;
        public DashboardController(ILogger<DashboardController> logger, IOrderService service,
            IScheduleService scheduleService)
        {
            _logger = logger;
            _orderService = service;
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.InCompletedOrders();
            ViewBag.TodayOrdersCount = await _orderService.TodayOrdersCount();
            ViewBag.PendingOrdersCount = await _orderService.PendingCount();
            ViewBag.TodayDeliveredCount = await _orderService.TodayDeliveredCount();
            ViewBag.TodaySales = await _orderService.TodaySale();
            ViewBag.isOpen = await _scheduleService.isShopOpen();
            return View(orders);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}