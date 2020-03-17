using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.DishService
{
    public class DishService : IDishService
    {
        private readonly AppDbContext _context;

        public DishService(AppDbContext context)
        {
            _context = context;
        }

        //Interface Functions
        public async Task<Dish> GetDish(int id)
        {
            var dish = await _context.Dishes.Include(s=>s.DishExtraTypes).ThenInclude(s=>s.DishExtras).SingleOrDefaultAsync(s=>s.DishId == id);
            return dish;
        }
        public async Task<List<Dish>> GetDishes(int id)
        {
            var dishes = await _context.Dishes.Where(s => s.DishCategoryId == id).ToListAsync();
            return dishes;
        }
        public async Task<List<Dish>> SearchWithCategory(int id)
        {
            var dish = await _context.Dishes.Where(s => s.DishCategoryId == id).ToListAsync();
            return dish;
        }
        public async Task<List<Dish>> AllDishes()
        {
            var AllDishes = await _context.Dishes.Include(s => s.DishCategory).Include(s=>s.DishExtraTypes).ThenInclude(s=>s.DishExtras).ToListAsync();
            return AllDishes;
        }
        public async Task EditDish(Dish editDish)
        {
            var dish = await _context.Dishes.FindAsync(editDish.DishId);

            dish.DishName = editDish.DishName;
            dish.SubName = editDish.SubName;
            dish.Allergies = editDish.Allergies;
            dish.Description = editDish.Description;
            dish.BasePrice = editDish.BasePrice;
            dish.Rating = editDish.Rating;

            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }
        public async Task AddDish(Dish addDish)
        {
            addDish.Status = true;

            await _context.Dishes.AddAsync(addDish);
            await _context.SaveChangesAsync();
        }
        public async Task AddDishExtra(DishExtra extra)
        {
            extra.IsAvailable = true;
            await _context.DishExtras.AddAsync(extra);
            await _context.SaveChangesAsync();
        }
        public async Task DisableDish(int id)
        {
            var dish = await GetDish(id);
            dish.Status = false;
            dish.DishExtraTypes.ToList().ForEach(s => s.Status = false);
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }
        public async Task EnableDish(int id)
        {
            var dish = await GetDish(id);
            dish.Status = true;
            dish.DishExtraTypes.ToList().ForEach(s => s.Status = true);
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }
        public async Task<List<DishExtraType>> GetDishExtrasTypes(int id)
        {
            var dishExtras = await _context.DishExtraTypes.Include(s => s.DishExtras).Where(s => s.DishId == id).ToListAsync();
            dishExtras.ForEach(s => s.DishExtras.ToList().ForEach(s => s.DishExtraType = null));
            return dishExtras;
        }
        public async Task EditDishExtraType(DishExtraType editDishExtraType)
        {
            var editExtraType = await _context.DishExtraTypes.FindAsync(editDishExtraType.DishExtraTypeId);
            editExtraType.TypeName = editDishExtraType.TypeName;
            editExtraType.ChooseMultiple = editDishExtraType.ChooseMultiple;
            editExtraType.Status = editDishExtraType.Status;
            _context.DishExtraTypes.Update(editExtraType);
            await _context.SaveChangesAsync();
        }
        public async Task EditDishExtra(DishExtra editDishExtra)
        {
            var dishExtra = await _context.DishExtras.FindAsync(editDishExtra.DishExtraId);
            dishExtra.ExtraName = editDishExtra.ExtraName;
            dishExtra.ExtraPrice = editDishExtra.ExtraPrice;
            _context.DishExtras.Update(dishExtra);
            await _context.SaveChangesAsync();
        }
        public async Task AddDishExtraTypes(int dishId, List<DishExtraType> dishExtraTypes)
        {
            foreach (var type in dishExtraTypes)
            {
                type.Status = true;
                type.DishId = dishId;

                await AddDishExtraType(type);
            }
        }
        
        //Helper Functions
        private async Task AddDishExtraType(DishExtraType dishExtraType)
        {
            await _context.DishExtraTypes.AddAsync(dishExtraType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDish(int id)
        {
            Dish dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeDishStatus(int id)
        {
            Dish dish = await _context.Dishes.FindAsync(id);
            if (dish.Status == true)
            {
                dish.Status = false;
                _context.Dishes.Update(dish);

                await _context.SaveChangesAsync();
            }
            else if (dish.Status == false)
            {
                dish.Status = true;
                _context.Dishes.Update(dish);
                await _context.SaveChangesAsync();
            }
        }
    }
}
