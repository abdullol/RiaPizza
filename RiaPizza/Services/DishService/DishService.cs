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
            var dish = await _context.Dishes.Include(s=>s.DishSizes).Include(s=>s.DishExtraTypes).ThenInclude(s=>s.DishExtras).SingleOrDefaultAsync(s=>s.DishId == id);
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

            
            await SaveToppingPrices(addDish);
        }
        private async Task SaveToppingPrices(Dish addDish)
        {
            try
            {
                var sizes = addDish.DishSizes;
                var toppingPrices = new List<SizeToppingPrice>();
                foreach (var extraType in addDish.DishExtraTypes)
                {
                    foreach (var extra in extraType.DishExtras)
                    {
                        foreach (var sizePrice in extra.SizeToppingPrices)
                        {
                            toppingPrices.Add(sizePrice);
                        }
                    }
                }
                foreach (var size in sizes)
                {
                    var toppingPrice = toppingPrices.Where(s => s.SizeName == size.Size).ToList();
                    toppingPrice.ForEach(s => s.DishSizeId = size.DishSizeId);

                    foreach (var price in toppingPrice)
                    {
                        _context.SizeToppingPrices.Update(price);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {

            }
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
            var dishExtras = await _context.DishExtraTypes.Include(s => s.DishExtras).ThenInclude(s=>s.SizeToppingPrices).Where(s => s.DishId == id).ToListAsync();
            dishExtras.ForEach(s => s.DishExtras.ToList().ForEach(s => { s.DishExtraType = null; s.SizeToppingPrices.ToList().ForEach(a => a.DishExtra = null); }));
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

        public async Task AddDishSize(DishSize size)
        {
            await _context.DishSize.AddAsync(size);
            await _context.SaveChangesAsync();
        }

        public async Task EditDishSize(DishSize size)
        {
            var dishSize = await _context.DishSize.FindAsync(size.DishSizeId);
            dishSize.Size = size.Size;
            dishSize.BasePrice = size.BasePrice;
            dishSize.Diameter = size.Diameter;

            _context.Update(dishSize);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishSize(int id)
        {
            var size = await _context.DishSize.FindAsync(id);
            _context.DishSize.Remove(size);
            await _context.SaveChangesAsync();
        }
    }
}
