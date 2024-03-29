﻿using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace RiaPizza.Services.DishCategoryService
{
    public class DishCategoryService : IDishCategoryService
    {
        private readonly AppDbContext _context;

        public DishCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddDishCategory(DishCategory addDishCategory)
        {
            addDishCategory.Status = true;
            addDishCategory.IsAvailable = true;

            await _context.DishCategories.AddAsync(addDishCategory);
            await _context.SaveChangesAsync();
        }

        public void AddUser(DishCategory images)
        {
            _context.DishCategories.Add(images);
            _context.SaveChanges();
        }

        public async Task<List<DishCategory>> AllDishCategories()
        {
            var allAreas = await _context.DishCategories.OrderBy(s=>s.OrderBy).ToListAsync();
            return allAreas;
        }

        public async Task<List<DishCategory>> CategoriesWithDishes()
        {
            try
            {
                var categories = await _context.DishCategories.Include(s => s.Dishes)
                    .ThenInclude(s => s.DishSizes).OrderBy(s=>s.OrderBy).ToListAsync();
                categories.ForEach(s => s.Dishes.ToList().ForEach(a => a.DishCategory = null ));
                categories.ForEach(s => s.Dishes.ToList().ForEach(a => a.DishSizes.ToList().ForEach(s => s.Dish = null)));
                foreach (var cat in categories)
                {
                    var dishes = cat.Dishes.Where(s => s.Status == true);
                    cat.Dishes = dishes;
                }
                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task EditDishCategory(DishCategory editDishCategory)
        {
            var dishCategory = await _context.DishCategories.FindAsync(editDishCategory.DishCategoryId);
            dishCategory.CategoryName = editDishCategory.CategoryName;
            dishCategory.Description = editDishCategory.Description;
            dishCategory.DishCategoryTax = editDishCategory.DishCategoryTax;
            if (editDishCategory.Image != null)
            {
                dishCategory.Image = editDishCategory.Image;
            }
            dishCategory.IsAvailable = editDishCategory.IsAvailable;
            dishCategory.OrderBy = editDishCategory.OrderBy;

            _context.DishCategories.Update(dishCategory);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleDishCategory(int id, bool isAvailable)
        {
            var dishCategory = await _context.DishCategories.FindAsync(id);
            dishCategory.IsAvailable = isAvailable;

            _context.DishCategories.Update(dishCategory);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            DishCategory dishCategory = await _context.DishCategories.FindAsync(id);
            _context.DishCategories.Remove(dishCategory);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeDishCategoryStatus(int id)
        {
            DishCategory dishCategory = await _context.DishCategories.FindAsync(id);
            if (dishCategory.Status == true)
            {
                dishCategory.Status = false;
                _context.DishCategories.Update(dishCategory);

                await _context.SaveChangesAsync();
            }
            else if (dishCategory.Status == false)
            {
                dishCategory.Status = true;
                _context.DishCategories.Update(dishCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
