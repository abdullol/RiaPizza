using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiaPizza.Services.DeliveryAreaService;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.DishService;
using RiaPizza.Services.ScheduleService;

namespace RiaPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDeliveryAreaService _areaService;
        private readonly IScheduleService _scheduleService;

        public HomeController(IDeliveryAreaService areaService, IScheduleService scheduleService)
        {
            _areaService = areaService;
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index(string error)
        {
            if (error != null)
            {
                ViewBag.Error = "Unfortunately your postcode is not in ours Delivery area......";
            }
            var areas = await _areaService.AllDeliveryAreas();

            ViewBag.ShopLogo = _scheduleService.GetSchedule().ShopLogo;
            return View(areas);
        }

    }
}
