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
                schedule.DeliveryTimings = timings.DeliveryTimings;

                _context.ShopSchedule.Update(schedule);
                await _context.SaveChangesAsync();
            }
            else
            {
                timings.IsOpen = false;
                await _context.ShopSchedule.AddAsync(timings);
                await _context.SaveChangesAsync();
            }

            HangfireService(schedule.DeliveryTimings);

        }

        private void HangfireService(IEnumerable<DeliveryTiming> timings)
        {
            foreach (var timing in timings)
            {
                var openShopExpression = "0 " + timing.TimeFrom + " * * " + (int)timing.DayOfWeek;
                var closeShopExpression = "0 " + timing.TimeTo + " * * " + (int)timing.DayOfWeek;

                RecurringJob.AddOrUpdate(timing.Day + "-Open", () => OpenShop(), openShopExpression, TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate(timing.Day + "-Close", () => CloseShop(), closeShopExpression, TimeZoneInfo.Local);
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

        public async Task<ShopSchedule> GetShopStatus()
        {
            try
            {
                var schedule = await _context.ShopSchedule.Include(s=>s.DeliveryTimings).FirstOrDefaultAsync();
                schedule.DeliveryTimings = schedule.DeliveryTimings.Where(s => s.DayOfWeek == DateTime.Now.DayOfWeek).ToList();
                schedule.DeliveryTimings.ToArray()[0].ShopSchedule = null;

                return schedule;
            }
            catch(Exception ex) { return null; }
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

        public async Task<ShopSchedule> GetSchedule()
        {
            var schedule = await _context.ShopSchedule.Include(s => s.DeliveryTimings).FirstOrDefaultAsync();
            schedule.DeliveryTimings.ToList().ForEach(s => s.ShopSchedule = null);
            return schedule;
        }
    }
}
