using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiaPizza.DTOs.Dish.ToppingSequence;
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
            catch (Exception ex)
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

                //var dishSizes = new List<DishSize>();
                //foreach (var item in dish.DishSizes)
                //{
                //    var size = new DishSize
                //    {
                //        BasePrice = item.BasePrice,
                //        Diameter = item.Diameter,
                //        Size = item.Size
                //    };
                //    dishSizes.Add(size);
                //}
                //cloneDish.DishSizes = dishSizes;

                //var dishExtraTypes = new List<DishExtraType>();
                //foreach (var item in dish.DishExtraTypes)
                //{
                //    var dishExtraType = new DishExtraType
                //    {
                //        ChooseMultiple = item.ChooseMultiple,
                //        Status = item.Status,
                //        TypeName = item.TypeName
                //    };

                //    var dishExtras = new List<DishExtra>();
                //    foreach (var extra in item.DishExtras)
                //    {
                //        var dishExtra = new DishExtra
                //        {
                //            ExtraName = extra.ExtraName,
                //            ExtraPrice = extra.ExtraPrice,
                //            Allergies = extra.Allergies,
                //            IsAvailable = extra.IsAvailable
                //        };

                //        var sizeToppingPrices = new List<SizeToppingPrice>();
                //        foreach (var topppingSize in extra.SizeToppingPrices)
                //        {
                //            var toppingPrice = new SizeToppingPrice
                //            {
                //                SizeName = topppingSize.SizeName,
                //                Price = topppingSize.Price
                //            };
                //            sizeToppingPrices.Add(toppingPrice);
                //        }
                //        dishExtra.SizeToppingPrices = sizeToppingPrices;
                //        dishExtras.Add(dishExtra);
                //    }
                //    dishExtraType.DishExtras = dishExtras;
                //    dishExtraTypes.Add(dishExtraType);
                //}
                //cloneDish.DishExtraTypes = dishExtraTypes;

                var dishSizes = new List<DishSize>();
                dish.DishSizes.ToList().ForEach(s =>
                {
                    var size = new DishSize
                    {
                        BasePrice = s.BasePrice,
                        Diameter = s.Diameter,
                        Size = s.Size
                    };
                    dishSizes.Add(size);
                });
                cloneDish.DishSizes = dishSizes;

                var dishExtraTypes = new List<DishExtraType>();
                dish.DishExtraTypes.ToList().ForEach(s =>
                {
                    var dishExtraType = new DishExtraType
                    {
                        ChooseMultiple = s.ChooseMultiple,
                        Status = s.Status,
                        TypeName = s.TypeName
                    };
                    var dishExtras = new List<DishExtra>();
                    s.DishExtras.ToList().ForEach(a =>
                    {
                        var dishExtra = new DishExtra
                        {
                            ExtraName = a.ExtraName,
                            ExtraPrice = a.ExtraPrice,
                            Allergies = a.Allergies,
                            IsAvailable = a.IsAvailable
                        };
                        var sizeToppingPrices = new List<SizeToppingPrice>();
                        a.SizeToppingPrices.ToList().ForEach(b =>
                        {
                            var toppingPrice = new SizeToppingPrice
                            {
                                SizeName = b.SizeName,
                                Price = b.Price
                            };
                            sizeToppingPrices.Add(toppingPrice);
                        });
                        dishExtra.SizeToppingPrices = sizeToppingPrices;
                        dishExtras.Add(dishExtra);
                    });
                    dishExtraType.DishExtras = dishExtras;
                    dishExtraTypes.Add(dishExtraType);
                });
                cloneDish.DishExtraTypes = dishExtraTypes;

                await _service.AddDish(cloneDish);
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }

        public async Task<IActionResult> GetLastElementEdit()
        {
            var lastElement = _service.GetLastAddedDish();
            var dish = await _service.GetDish(lastElement.DishId);
            ViewBag.Categories = await _categoryService.AllDishCategories();
            return View("Edit", dish);
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
                if (extra.DishExtraId == 0)
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
            if (area == null)
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

        public async Task<IActionResult> DeleteSize(int id)
        {
            await _service.DeleteDishSize(id);
            return RedirectToAction("Index", "Dishes");
        }

        public async Task<IActionResult> DeleteDishes(int[] dishIds)
        {
            await _service.DelMultipleDish(dishIds);
            return RedirectToAction("Index");
        }

        public async Task DeleteDishExtra(int DishExtraId)
        {
            try
            {
                await _service.DeleteDishExtra(DishExtraId);
            }
            catch (Exception ex)
            {
            }
        }
        public async Task DeleteDishExtraType(int DishExtraTypeId)
        {
            try
            {
                await _service.DeleteDishExtraType(DishExtraTypeId);
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateToppingSequence(IFormCollection form)
        {
            var dishId = JsonConvert.DeserializeObject<int>(form["dishId"]);
            try
            {
                var dishExtraTypes = JsonConvert.DeserializeObject<IEnumerable<ExtraTypeSequenceDto>>(form["toppingSequence"]);
                await _service.UpdateToppingSequence(dishExtraTypes, dishId);
                return RedirectToAction("Edit", "Dishes", new { @id = dishId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Dishes", new { @id = dishId });
            }
        }

        public async Task UpdateDishExtraTypes(IFormCollection form)
        {
            try
            {
                var id = JsonConvert.DeserializeObject<int>(form["dishId"]);
                var dishExtraTypes = JsonConvert.DeserializeObject<List<DishExtraType>>(form["dishExtraTypes"]);
                await _service.UpdateDishExtraTypes(id, dishExtraTypes);

            }
            catch (Exception ex)
            {

            }
        }
    }
}