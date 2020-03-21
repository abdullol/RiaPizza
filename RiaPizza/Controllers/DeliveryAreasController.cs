using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RiaPizza.Data;
using RiaPizza.Models;
using RiaPizza.Services.DeliveryAreaService;

namespace RiaPizza.Controllers
{
    public class DeliveryAreasController : Controller
    {
        private readonly IDeliveryAreaService _service;

        public DeliveryAreasController(IDeliveryAreaService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetAllAreas()
        {
            var allAreas = await _service.AllDeliveryAreas();
            return Json(allAreas);
        }

        
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> Create(IFormCollection form)
        {
            var deliveryArea = JsonConvert.DeserializeObject<DeliveryArea>(form["deliveryArea"]);
            if (await _service.ValidateArea(deliveryArea.AreaName))
            {
                return Json("PostalCodeExists");
            }
            else
            {
                await _service.AddDeliveryArea(deliveryArea);
                return Json("Success");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> Edit(IFormCollection form)
        {
            var deliveryArea = JsonConvert.DeserializeObject<DeliveryArea>(form["deliveryArea"]);
            string message;
            try
            {
                if (await _service.AreaOtherThanThis(deliveryArea.AreaName, deliveryArea.DeliveryAreaId))
                {
                    return Json("PostalCodeExists");
                }
                else
                {
                    await _service.EditDeliveryArea(deliveryArea);
                    message = "Success";
                }
            }
            catch(Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message);
        }

        public async Task<JsonResult> Delete(int id)
        {
            await _service.Delete(id);
            return Json("Success");
        }
        public async Task<ActionResult> ChangeStatus(int id) 
        {
          await  _service.ChangeAreaStatus(id);
            return RedirectToAction("Index");
        }

    }
}
