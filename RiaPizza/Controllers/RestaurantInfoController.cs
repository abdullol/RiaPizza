using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiaPizza.Models;
using RiaPizza.Services.RestaurantInfoService;

namespace RiaPizza.Controllers
{
    public class RestaurantInfoController : Controller
    {
        private readonly IRestaurantInfoService _restaurantInfoService;
        public RestaurantInfoController(IRestaurantInfoService restaurantInfoService)
        {
            _restaurantInfoService = restaurantInfoService;
        }
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostLocation(RestaurantInfo model)
        {
            await _restaurantInfoService.AddLocation(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> PostOwnerInfo(RestaurantInfo model)
        {
            await _restaurantInfoService.AddOwnerDetails(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<JsonResult> GetLocation()
        {
            var location = await _restaurantInfoService.GetLocation();
            return Json(location.RestaurantLocation);
        }
        [HttpPost]
        public async Task<JsonResult> GetOwnerDetails()
        {
            var location = await _restaurantInfoService.GetLocation();
            return Json(location.OwnerDetails);
        }
    }
}
