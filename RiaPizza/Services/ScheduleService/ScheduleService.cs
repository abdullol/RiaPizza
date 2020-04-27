using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> isShopOpen()
        {
            var schedule = await _context.ShopSchedule.FirstOrDefaultAsync();
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

                _context.ShopSchedule.Update(schedule);
                await _context.SaveChangesAsync();
            }
            else
            {
                timings.IsOpen = false;
                await _context.ShopSchedule.AddAsync(timings);
                await _context.SaveChangesAsync();
            }

            var openShopExpression = "0 "+ schedule.TimeFrom.Hours + " * * *";
            var closeShopExpression = "0 " + schedule.TimeTo.Hours + " * * *";

            RecurringJob.AddOrUpdate("OpenShop",() =>  OpenShop(), openShopExpression, TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate("CloseShop", () => CloseShop(), closeShopExpression, TimeZoneInfo.Local);
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

        public async Task<ShopSchedule> GetSchedule()
        {
            var schedule = await _context.ShopSchedule.FirstOrDefaultAsync();
            return schedule;
        }

        public async Task OpenShop()
        {
            var schedule = await _context.ShopSchedule.FirstOrDefaultAsync();
            schedule.IsOpen = true;
            _context.ShopSchedule.Update(schedule);
            await _context.SaveChangesAsync();
        }
        public async Task CloseShop()
        {
            var schedule = await _context.ShopSchedule.FirstOrDefaultAsync();
            schedule.IsOpen = false;
            _context.ShopSchedule.Update(schedule);
            await _context.SaveChangesAsync();
        }
    }
}
