using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiaPizza.Data;
using RiaPizza.Models;
using RiaPizza.Services.DeliveryTimingService;

namespace RiaPizza.Controllers
{
    public class DeliveryTimingController : Controller
    {
        private readonly IDeliveryTimingService _Service;
        public DeliveryTimingController(IDeliveryTimingService Service)
        {
            _Service = Service;
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Index()
        {
            var getAllTiming = await  _Service.GetAllTiming();
            return View(getAllTiming);
        }

        public async Task<JsonResult> GetDeliveryTimes()
        {
            var getAllTiming = await _Service.GetAllTiming();
            return Json(getAllTiming);
        }

        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DeliveryTiming deliveryTiming)
        {
            try
            {
                // TODO: Add insert logic here
               await _Service.AddDeliveryTiming(deliveryTiming);
               return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Edit(int id)
        {
             var editdeliveryTiming =await _Service.GetById(id);
            return View(editdeliveryTiming);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DeliveryTiming deliveryTiming)
        {
            try
            {
                // TODO: Add update logic here
                await _Service.UpdateDeliveryTiming(deliveryTiming);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Delete(int id)
        {
           await _Service.DeleteDeliveryTiming(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}