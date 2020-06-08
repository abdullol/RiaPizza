using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RiaPizza.Data;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.Models;
using RiaPizza.Services.AccountService;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.DishService;
using RiaPizza.Services.NotifyOrder;
using RiaPizza.Services.OrderService;
using RiaPizza.Services.ScheduleService;
using RiaPizza.Services.RenderViewService;
using RiaPizza.DTOs.DishInvoice;

namespace RiaPizza.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _service;
        private readonly IAccountService _accountService;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDishService _dishService;
        private readonly IDishCategoryService _dishCatService;
        private readonly IScheduleService _scheduleService;
        public OrdersController(
            IOrderService service,
            IAccountService accountService,
            IHubContext<NotifyHub> hubContext,
            UserManager<AppUser> userManager,
            IDishService dishService,
            IDishCategoryService dishCatService,
            IScheduleService scheduleService
            )
        {
            _service = service;
            _accountService = accountService;
            _hubContext = hubContext;
            _userManager = userManager;
            _dishService = dishService;
            _dishCatService = dishCatService;
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _service.AllOrders();
            if (orders == null)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> Reports(List<Order> order)
        {
            ViewBag.DishList = await _dishService.AllDishes();
            ViewBag.DishCatList = await _dishCatService.AllDishCategories();
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(order);
        }

        [HttpPost]
        [ActionName("ReportByDate")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reports(string DF, string DT, string status)
        {
            ViewBag.DishList = await _dishService.AllDishes();
            ViewBag.DishCatList = await _dishCatService.AllDishCategories();
            ViewBag.Users = await _userManager.Users.ToListAsync();
            var orderSearch = await _service.SearchByDate(DF, DT);
            if (status != null && status != "")
            {
                var statusFiltered = orderSearch.Where(s => s.OrderStatus == status).ToList();
                return View("Reports", statusFiltered);
            }
            return View("Reports", orderSearch);
        }

        [HttpPost]
        [ActionName("ReportByFilter")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reports(string status, string number, string address, int DishId, int DishCatId, int userId)
        {
            ViewBag.DishList = await _dishService.AllDishes();
            ViewBag.DishCatList = await _dishCatService.AllDishCategories();
            ViewBag.Users = await _userManager.Users.ToListAsync();

            if (status != null)
            {
                var orderStatus = await _service.SearchByStatus(status);
                return View("Reports", orderStatus);
            }
            else if (number != null)
            {
                var orderByNumber = await _service.SearchByNumber(number);
                return View("Reports", orderByNumber);
            }
            else if (DishId != 0)
            {
                var searchByDish = await _service.SearchByDish(DishId);
                return View("Reports", searchByDish);
            }
            else if (DishCatId != 0)
            {
                var searchByDishCat = await _service.SearchByDishCat(DishCatId);
                return View("Reports", searchByDishCat);
            }
            else if (address != null)
            {
                var searchByAddress = await _service.SearchByAddress(address);
                return View("Reports", searchByAddress);
            }
            else if (userId != 0)
            {
                //var usersNotInRole = context.Users.Where(m => m.Roles.All(r => r.RoleId != role.Id));
                var serchByUser = await _service.SearchByUser(userId);
                return View("Reports", serchByUser);
            }
            return View();
        }

        [HttpPost]
        [ActionName("ReportFilter")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reports(string DF, string DT, string status, string number, string address, int DishId, int DishCatId, int userId, string payment)
        {
            ViewBag.DishList = await _dishService.AllDishes();
            ViewBag.DishCatList = await _dishCatService.AllDishCategories();
            ViewBag.Users = await _userManager.Users.ToListAsync();
            var orderSearch = await _service.Filter(DF, DT, status, number, address, DishId, DishCatId, userId, payment);

            return View("Reports", orderSearch);
        }

        [HttpPost]
        public async Task<JsonResult> Create(IFormCollection form)
        {
            try
            {
                var order = JsonConvert.DeserializeObject<Order>(form["order"]);
                var addAddress = JsonConvert.DeserializeObject<bool>(form["addAddress"]);
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

                return Json(order);
                //if (orderId != 0)
                //{ 
                //    var user = await _userManager.GetUserAsync(User);
                //    var email = new MimeMessage();
                //    email.From.Add(new MailboxAddress("RiaPizza", "xyz@gmail.com"));
                //    email.To.Add(new MailboxAddress("User", user.Email));
                //    email.Subject = "Test Message";
                //    email.Body = new TextPart("Plain")
                //    {
                //        Text = "you have place order, your order is confiremed"
                //    };

                //    using (var client = new MailKit.Net.Smtp.SmtpClient())
                //    {
                //        client.Connect("smtp.gmail.com", 587, false);
                //        //SMTP server authentication if needed
                //        client.Authenticate("xyz@gmail.com", "");

                //        client.Send(email);

                //        client.Disconnect(true);
                //    };
                //        return Json(order);
                //}
                //    else
                //        return Json("Failed");

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
        public async Task<ContentResult> GetOrderDetails(int id)
        {
            var orderDetails = await _service.GetOrderDetails(id);
            var result = await this.RenderViewToStringAsync("/Views/Orders/_OrderDetails.cshtml", orderDetails);
            return Content(result);
        }


        public async Task<JsonResult> GetUserOrders(int id)
        {
            var orders = await _service.GetUserOrders(id);
            return Json(orders);
        }

        public async Task<JsonResult> GetUserOrdersFromCodes(IFormCollection form)
        {
            var list = JsonConvert.DeserializeObject<List<string>>(form["orderCodes"]).ToList();
            var orders = await _service.GetUserOrdersFromCodes(list);
            return Json(orders);
        }

        public async Task<IActionResult> ThankYou(string address, int id)
        {
            var isCompleted = await _service.IsCompleted(id);
            if (!isCompleted)
            {
                var orderItems = await _service.GetOrderItems(id);
                ViewBag.Address = address;
                ViewBag.OrderCode = await _service.GetOrderCode(id);
                return View(orderItems);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteOrder(id);
            return RedirectToAction("Index", "Orders");
        }


        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Invoice(int id)
        {
            var order = await _service.GetOrder(id);
            return View(order);
        }

        public async Task<IActionResult> SecondInvoice(int id)
        {
            var order = await _service.GetOrder(id);
            return View(order);
        }
    }
}