using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiaPizza.Data;
using RiaPizza.Models;
using RiaPizza.Services.CouponService;

namespace RiaPizza.Controllers
{
    public class CouponsController : Controller
    {
        private readonly ICouponService _service;
        public CouponsController(ICouponService service)
        {
            _service = service;
        }
        // GET: Coupons
        public async Task<JsonResult> Index()
        {
            var allCoupons= await _service.GetAllCoupons();
            return Json(allCoupons);
        }


        // POST: Coupons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(IFormCollection form)
        {
            var addCoupon = JsonConvert.DeserializeObject<Coupon>(form["coupon"]);
            string message;
            try
            {
                // TODO: Add insert logic here
                await _service.AddCoupon(addCoupon);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message);
        }


        // POST: Coupons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(IFormCollection form)
        {
            var editCoupon = JsonConvert.DeserializeObject<Coupon>(form["coupon"]);
            string message;
            try
            {
                // TODO: Add update logic here
                await _service.EditCoupon(editCoupon);
                message = "Success";
                
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message);
        }

        
        public async Task<JsonResult> Delete(int id)
        {
            string message;
            try
            {
                // TODO: Add delete logic here
               await _service.DeleteCoupon(id);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message);
        }
    }
}