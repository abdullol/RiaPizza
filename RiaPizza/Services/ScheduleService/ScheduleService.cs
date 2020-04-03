using Microsoft.AspNetCore.Http;
using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.ScheduleService
{
    public class ScheduleService : IScheduleService
    {
        private readonly AppDbContext _context;
        public ScheduleService(AppDbContext context)
        {
            _context = context;
        }

        public bool isShopOpen()
        {
            var schedule = _context.ShopSchedule.FirstOrDefault();
            return schedule.IsOpen;
        }

        public async Task DeleteSchedule()
        {
            var schedule = _context.ShopSchedule.FirstOrDefault();
            _context.ShopSchedule.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task AddorUpdate(ShopSchedule timings)
        {
            var schedule = _context.ShopSchedule.FirstOrDefault();
            if(schedule != null)
            {
                schedule.TimeFrom = timings.TimeFrom;
                schedule.TimeTo = timings.TimeTo;
                schedule.ShopLogo = timings.ShopLogo;

                _context.ShopSchedule.Update(schedule);
                await _context.SaveChangesAsync();
            }
            else
            {
                timings.IsOpen = false;
                await _context.ShopSchedule.AddAsync(timings);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleShop(string status)
        {
            var schedule = _context.ShopSchedule.FirstOrDefault();

            if (status == "on")
                schedule.IsOpen = true;
            else
                schedule.IsOpen = false;

            _context.ShopSchedule.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public ShopSchedule GetSchedule()
        {
            var schedule = _context.ShopSchedule.FirstOrDefault();
            return schedule;
        }
    }
}
