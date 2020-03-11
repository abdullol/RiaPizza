using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.DishService
{
    public interface IDishService
    {
        Task<Dish> GetDish(int id);
        Task<List<Dish>> GetDishes(int id);
        Task<List<Dish>> SearchWithCategory(int id);
        Task AddDish(Dish addDish);
        Task EditDish(Dish editDish);
        Task<List<Dish>> AllDishes();
        Task<List<DishExtraType>> GetDishExtrasTypes(int id);
        Task DisableDish(int id);
        Task EnableDish(int id);
        Task EditDishExtraType(DishExtraType editDishExtraType);
        Task EditDishExtra(DishExtra editDishExtra);
        Task AddDishExtraTypes(int dishId, List<DishExtraType> dishExtraTypes);
        Task AddDishExtra(DishExtra extra);
        Task DeleteDish(int id);
    }
}
