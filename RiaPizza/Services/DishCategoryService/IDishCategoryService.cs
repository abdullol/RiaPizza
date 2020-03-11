using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.DishCategoryService
{
    public interface IDishCategoryService
    {
        Task AddDishCategory(DishCategory addDishCategory);
        Task EditDishCategory(DishCategory editDishCategory);
        Task ToggleDishCategory(int id, bool isAvailable);
        Task<List<DishCategory>> AllDishCategories();
        Task<List<DishCategory>> CategoriesWithDishes();
        Task Delete(int id);
    }
}
