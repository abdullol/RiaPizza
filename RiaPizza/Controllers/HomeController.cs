using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiaPizza.Services.DeliveryAreaService;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.DishService;

namespace RiaPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDeliveryAreaService _areaService;
        
        public HomeController(IDeliveryAreaService areaService)
        {
            _areaService = areaService;
        }

        public async Task<IActionResult> Index(string error)
        {
            if (error != null)
            {
                ViewBag.Error = "No Area Found!";
            }
            var areas = await _areaService.AllDeliveryAreas();
            return View(areas);
        }

    }
}
