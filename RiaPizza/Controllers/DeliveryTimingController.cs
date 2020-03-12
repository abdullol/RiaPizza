using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // GET: DeliveryTiming
        public async Task<ActionResult> Index()
        {
            var getAllTiming = await  _Service.GetAllTiming();
            return View(getAllTiming);
        }


        // GET: DeliveryTiming/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryTiming/Create
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

        // GET: DeliveryTiming/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
             var editdeliveryTiming =await _Service.GetById(id);
            return View(editdeliveryTiming);
        }

        // POST: DeliveryTiming/Edit/5
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

        // GET: DeliveryTiming/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
           await _Service.DeleteDeliveryTiming(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: DeliveryTiming/Delete/5
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