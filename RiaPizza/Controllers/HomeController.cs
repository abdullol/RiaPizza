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
                ViewBag.Error = "Leider befindet sich Ihre Postleitzahl nicht in unserem Lieferbereich......";
            }
            var areas = await _areaService.AllDeliveryAreas();

            return View(areas);
        }

    }
}
