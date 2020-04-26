using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiaPizza.Models;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.ScheduleService;

namespace RiaPizza.Controllers
{
    public class DishCategoriesController : Controller
    {
        private readonly IDishCategoryService _service;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IScheduleService _scheduleService;

        public DishCategoriesController(
            IDishCategoryService service,
            IHostingEnvironment hostingEnvironment, IScheduleService scheduleService)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> ToggleDishCategories(int id)
        {
            await _service.ChangeDishCategoryStatus(id);
            return RedirectToAction("Index", "DishCategories");
        }

        public async Task<JsonResult> Delete(int id)
        {
            await _service.Delete(id);
            return Json("Success");
        }

        public async Task<JsonResult> GetAllCategories()
        {
            var allCategories = await _service.AllDishCategories();
            return Json(allCategories);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<JsonResult> Edit()
        {
            string message;
            try
            {
                var dishCategory = JsonConvert.DeserializeObject<DishCategory>(Request.Form["dishcategory"]);
                if (Request.Form.Files.Count > 0)
                {
                    string UniqueFilename;
                    var file = Request.Form.Files[0];
                    string UploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

                    if (!Directory.Exists(UploadFolder))
                    {
                        Directory.CreateDirectory(UploadFolder);
                    }

                    UniqueFilename = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(UploadFolder, UniqueFilename);
                    file.CopyTo(new FileStream(filePath, FileMode.Create));
                    dishCategory.Image = UniqueFilename;
                }
                await _service.EditDishCategory(dishCategory);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Json(message);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ContentResult> Create()
        {
            var category = JsonConvert.DeserializeObject<DishCategory>(Request.Form["category"]); ;
            var file = Request.Form.Files[0];
            string UniqueFilename;
            if (file != null)
            {
                string UploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

                if (!Directory.Exists(UploadFolder))
                {
                    Directory.CreateDirectory(UploadFolder);
                }

                UniqueFilename = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(UploadFolder, UniqueFilename);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                category.Image = UniqueFilename;
                await _service.AddDishCategory(category);
            }
            return Content("Success");
        }
    }
}