﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiaPizza.Models;
using RiaPizza.Services.DeliveryAreaService;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.DishService;

namespace RiaPizza.Controllers
{
    public class DishesController : Controller
    {
        private readonly IDishService _service;
        private readonly IDishCategoryService _categoryService;
        private readonly IDeliveryAreaService _areaService;
        public DishesController(IDishService service, IDishCategoryService categoryService, IDeliveryAreaService areaService)
        {
            _service = service;
            _categoryService = categoryService;
            _areaService = areaService;
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.Categories = await _categoryService.AllDishCategories();
            if (id == null || id == 0)
            {
                var dishes = await _service.AllDishes();
                return View(dishes);
            }
            else
            {
                var dishes = await _service.SearchWithCategory(Convert.ToInt32(id));
                return View(dishes);
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> CreateDish(IFormCollection form)
        {
            try
            {
                var dish = JsonConvert.DeserializeObject<Dish>(form["dish"]);
                var dishExtraTypes = JsonConvert.DeserializeObject<List<DishExtraType>>(form["dishExtraTypes"]);

                dish.DishExtraTypes = dishExtraTypes;

                await _service.AddDish(dish);
                return Json("Success");
            }
            catch(Exception ex)
            {
                return Json("Failed");
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _service.GetDish(id);
            ViewBag.Categories = await _categoryService.AllDishCategories();
            return View(dish);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Edit(Dish dish)
        {
            await _service.EditDish(dish);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditDishExtraType(DishExtraType extra, int returnId)
        {
            try
            {
                await _service.EditDishExtraType(extra);
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            } 
        }

        [HttpPost]
        public async Task<IActionResult> EditDishExtra(DishExtra extra, int returnId)
        {
            try
            {
                await _service.EditDishExtra(extra);
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddDishExtras(IFormCollection form)
        {
            try
            {
                var id = JsonConvert.DeserializeObject<int>(form["dishId"]);
                var dishExtraTypes = JsonConvert.DeserializeObject<List<DishExtraType>>(form["dishExtraTypes"]);
                await _service.AddDishExtraTypes(id, dishExtraTypes);
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDishExtra(DishExtra extra, int returnId)
        {
            try
            {
                await _service.AddDishExtra(extra);
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisableDish(int id)
        {
            await _service.DisableDish(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EnableDish(int id)
        {
            await _service.EnableDish(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddDishSize(DishSize dishSize)
        {
            try
            {
                await _service.AddDishSize(dishSize);
                return RedirectToAction("Edit", "Dishes", new { @id = dishSize.DishId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = dishSize.DishId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDishSize(DishSize dishSize)
        {
            try
            {
                await _service.EditDishSize(dishSize);
                return RedirectToAction("Edit", "Dishes", new { @id = dishSize.DishId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = dishSize.DishId });
            }
        }

        public async Task<IActionResult> Explore(string postalCode)
        {
            var area = await _areaService.GetDeliveryArea(postalCode);
            if(area == null)
            {
                ViewBag.Error = "No Area Found!";
                return RedirectToAction("Index", "Home", new { error = "Error" });
            }
            ViewBag.Categories = await _categoryService.AllDishCategories();
            return View(area);
        }

        public async Task<JsonResult> GetCategories()
        {
            var dishes = await _categoryService.CategoriesWithDishes();
            return Json(dishes);
        }

        public async Task<JsonResult> GetDishesExtraTypes(int id)
        {
            var dishes = await _service.GetDishExtrasTypes(id);
            return Json(dishes);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteDish(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleDish(int id)
        {
            await _service.ChangeDishStatus(id);
            return RedirectToAction("Index", "Dishes");
        }
    }
}