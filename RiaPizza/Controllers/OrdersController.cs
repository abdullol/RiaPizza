using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RiaPizza.Data;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.Models;
using RiaPizza.Services.AccountService;
using RiaPizza.Services.NotifyOrder;
using RiaPizza.Services.OrderService;

namespace RiaPizza.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _service;
        private readonly IAccountService _accountService;
        private readonly IHubContext<NotifyHub> _hubContext;
        public OrdersController(
            IOrderService service,
            IAccountService accountService,
            IHubContext<NotifyHub> hubContext
            )
        {
            _service = service;
            _accountService = accountService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _service.AllOrders();
            return View(orders);
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> SearchTodayOrders(string filter)
        {
            if (filter == "TodayOrders" || filter == null)
            {
                var todayOrders = await _service.TodayOrders();
                ViewBag.Filter = "Today Orders";
                return View(todayOrders);
            }
            else if (filter == "Pending")
            {
                var processing = await _service.PendingOrders();
                ViewBag.Filter = "Pending Orders";
                return View(processing);
            }
            else if (filter == "Delivered")
            {
                var delivered = await _service.TodayDeliveredOrders();
                ViewBag.Filter = "Today Delivered";
                return View(delivered);
            }
            else if (filter == "TodaySale")
            {
                var todaySale = await _service.TotalTodaySales();
                ViewBag.Filter = "Today Sale";
                return View(todaySale);
            }

            return View();
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Invoice(int id)
        {
            var order = await _service.GetOrder(id);
            return View(order);
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> OrdersInProgress()
        {
            var uncompletedOrders = await _service.InCompletedOrders();
            return View(uncompletedOrders);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> ChangeOrderStatus(int orderId, string orderStatus)
        {
            await _service.ChangeStatus(orderId, orderStatus);
            return Json(orderId);
        }

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Reports(List<Order> order)
        {
            return View(order);
        }

        [HttpPost]
        [ActionName("ReportByDate")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reports(string DF, string DT, string status)
        {
            var orderSearch = await _service.SearchByDate(DF, DT);
            if(status != null && status != "")
            {
                var statusFiltered = orderSearch.Where(s => s.OrderStatus == status).ToList();
                return View("Reports", statusFiltered);
            }
            return View("Reports", orderSearch);
        }

        [HttpPost]
        [ActionName("ReportByStatus")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reports(string status)
        {
            var orderStatus = await _service.SearchByStatus(status);
            return View("Reports", orderStatus);
        }

        [HttpPost]
        public async Task<JsonResult> Create(IFormCollection form)
        {
            var order = JsonConvert.DeserializeObject<Order>(form["order"]);
            var addAddress = JsonConvert.DeserializeObject<bool>(form["addAddress"]);
            try
            {
                if (addAddress)
                {
                    var userAddress = new AppUserAddress
                    {
                        Address = order.OrderDeliveryAddress.Address,
                        City = order.OrderDeliveryAddress.City,
                        PostalCode = order.OrderDeliveryAddress.PostalCode,
                        Floor = order.OrderDeliveryAddress.Floor,
                        UserId = Int32.Parse(order.UserId.ToString()),
                    };
                    await _accountService.AddUserAddress(userAddress);
                }
                var orderId = await _service.CreateOrder(order);
                order.OrderBy.Order = null;
                order.OrderDeliveryAddress.Order = null;
                order.OrderItems.ToList().ForEach(s => s.Order = null);
                await _hubContext.Clients.All.SendAsync("notifyOrder", order);

                if (orderId != 0)
                    return Json(order);
                else
                    return Json("Failed");

            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> GetOrderDetails(int id)
        {
            var orderDetails = await _service.GetOrderDetails(id);
            return Json(orderDetails);
        }

        public async Task<JsonResult> GetUserOrders(int id)
        {
            var orders = await _service.GetUserOrders(id);
            return Json(orders);
        }

        public async Task<IActionResult> ThankYou(string postalCode, int id)
        {
            var isCompleted = await _service.IsCompleted(id);
            if (!isCompleted)
            {
                var orderItems = await _service.GetOrderItems(id);
                ViewBag.PostalCode = postalCode;
                ViewBag.OrderCode = await _service.GetOrderCode(id);
                return View(orderItems);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SecondInvoice()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteOrder(id);
            return RedirectToAction("Index", "Orders");
        }
      
    }
}