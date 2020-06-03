using RiaPizza.DTOs.Dish.ToppingSequence;
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
        Task<DishExtra> GetDishExtra(int id);
        Task<List<Dish>> GetDishes(int id);
        Task<List<DishSize>> GetDisheSizes(int id);
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
        Task UpdateDishExtraTypes(int dishId, List<DishExtraType> dishExtraTypes);
        Task AddDishExtra(DishExtra extra);
        Task DeleteDish(int id);
        Task AddDishSize(DishSize size);
        Task EditDishSize(DishSize size);
        Task DeleteDishSize(int id);
        Task DeleteDishExtra(int id);
        Task DeleteDishExtraType(int id);
        Task DelMultipleDish(int[] dishIds);
        Task AddDishExtraType(DishExtraType dishExtraType);
        Task UpdateToppingSequence(IEnumerable<ExtraTypeSequenceDto> sequenceDto, int dishId);
        Dish GetLastAddedDish();
    }
}
