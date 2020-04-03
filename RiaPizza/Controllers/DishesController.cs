using System;
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
using RiaPizza.Services.ScheduleService;

namespace RiaPizza.Controllers
{
    public class DishesController : Controller
    {
        private readonly IDishService _service;
        private readonly IDishCategoryService _categoryService;
        private readonly IDeliveryAreaService _areaService;
        private readonly IScheduleService _scheduleService;

        public DishesController(IDishService service, IDishCategoryService categoryService, IDeliveryAreaService areaService, IScheduleService scheduleService)
        {
            _service = service;
            _categoryService = categoryService;
            _areaService = areaService;
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.Categories = await _categoryService.AllDishCategories();
            if (id == null || id == 0)
            {
                var dishes = await _service.AllDishes();
                ViewBag.ShopLogo = _scheduleService.GetSchedule().ShopLogo;
                return View(dishes);
            }
            else
            {
                var dishes = await _service.SearchWithCategory(Convert.ToInt32(id));
                ViewBag.ShopLogo = _scheduleService.GetSchedule().ShopLogo;
                return View(dishes);
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Add()
        {
            ViewBag.ShopLogo = _scheduleService.GetSchedule().ShopLogo;
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

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> DuplicateDish(int id)
        {
            try
            {
                var dish = await _service.GetDish(id);

                var cloneDish = new Dish
                {
                    Allergies = dish.Allergies,
                    BasePrice = dish.BasePrice,
                    Description = dish.Description,
                    DishCategoryId = dish.DishCategoryId,
                    DishName = dish.DishName,
                    SubName = dish.SubName,
                    Rating = dish.Rating
                };
                cloneDish.DishSizes = new List<DishSize>();
                dish.DishSizes.ToList().ForEach(s =>
                {
                    var size = new DishSize 
                    {
                        BasePrice = s.BasePrice,
                        Diameter = s.Diameter,
                        Size = s.Size
                    };

                    cloneDish.DishSizes.ToList().Add(size);
                });
                cloneDish.DishExtraTypes = new List<DishExtraType>();
                dish.DishExtraTypes.ToList().ForEach(s =>
                {
                    var dishExtraType = new DishExtraType
                    {
                        ChooseMultiple = s.ChooseMultiple,
                        Status = s.Status,
                        TypeName = s.TypeName
                    };
                    dishExtraType.DishExtras = new List<DishExtra>();
                    s.DishExtras.ToList().ForEach(a =>
                    {
                        var dishExtra = new DishExtra
                        {
                            ExtraName = a.ExtraName,
                            ExtraPrice = a.ExtraPrice,
                            Allergies = a.Allergies,
                            IsAvailable = a.IsAvailable
                        };
                        dishExtra.SizeToppingPrices = new List<SizeToppingPrice>();
                        a.SizeToppingPrices.ToList().ForEach(b =>
                        {
                            var toppingPrice = new SizeToppingPrice
                            {
                                SizeName = b.SizeName,
                                Price = b.Price
                            };
                            dishExtra.SizeToppingPrices.ToList().Add(toppingPrice);
                        });

                        dishExtraType.DishExtras.ToList().Add(dishExtra);
                    });
                    cloneDish.DishExtraTypes.ToList().Add(dishExtraType);
                });

                await _service.AddDish(cloneDish);
                return Json("Success");
            }
            catch (Exception ex)
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
        public async Task<JsonResult> AddorUpdateDishExtra(IFormCollection form)
        {
            try
            {
                var extra = JsonConvert.DeserializeObject<DishExtra>(form["extra"]);
                if(extra.DishExtraId == 0)
                    await _service.AddDishExtra(extra);
                else
                    await _service.EditDishExtra(extra);

                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }

        public async Task<JsonResult> GetDishExtra(int id)
        {
            var extra = await _service.GetDishExtra(id);
            return Json(extra);
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
        public async Task<IActionResult> AddDishExtraType(DishExtraType extra, int returnId)
        {
            try
            {
                await _service.AddDishExtraType(extra);
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

        public async Task<JsonResult> GetDishSizes(int id)
        {
            var sizes = await _service.GetDisheSizes(id);
            return Json(sizes);
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

        public async Task<IActionResult> Explore(string areaname)
        {
            var area = await _areaService.GetDeliveryArea(areaname);
            if(area == null)
            {
                ViewBag.Error = "No Area Found!";
                return RedirectToAction("Index", "Home", new { error = "Error" });
            }
            ViewBag.Categories = await _categoryService.AllDishCategories();
            ViewBag.ShopLogo = _scheduleService.GetSchedule().ShopLogo;
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

        public async Task<IActionResult> DeleteDishes(int[] dishIds)
        {
            await _service.DelMultipleDish(dishIds);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteDishExtra(int DishExtraId, int returnId)
        {
            try
            {
                await _service.DeleteDishExtra(DishExtraId);
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
        }
        public async Task<IActionResult> DeleteDishExtraType(int DishExtraTypeId, int returnId)
        {
            try
            {
                await _service.DeleteDishExtraType(DishExtraTypeId);
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = returnId });
            }
        }


    }
}