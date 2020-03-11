using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace RiaPizza.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Denied()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.HomePath = "~/Dashboard/Index";
            }
            else if (User.IsInRole("Manager"))
            {
                ViewBag.HomePath = "~/Dashboard/Index";
            }
            else if (User.IsInRole("Rider"))
            {
                ViewBag.HomePath = "~/Orders/OrdersInProgress";
            }
            return View();
        }
    }
}