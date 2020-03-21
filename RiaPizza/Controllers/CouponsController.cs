﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Index()
        {
            var allCoupons= await _service.GetAllCoupons();
            return View(allCoupons);
        }
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Create() 
        {
            return View();
        }


        // POST: Coupons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create(Coupon addCoupon)
        {
           // var addCoupon = JsonConvert.DeserializeObject<Coupon>(form["coupon"]);
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
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Edit(int id) 
        {
           var coupon=await _service.GetById(id);
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(Coupon editCoupon)
        {
           // var editCoupon = JsonConvert.DeserializeObject<Coupon>(form["coupon"]);
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
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Delete(int id)
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
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> ValidateCoupon(string code)
        {
            var _code = await _service.ValidateCoupon(code);
            if (_code == null)
                return Json(false);
            else
                return Json(_code);
        }

    }
}