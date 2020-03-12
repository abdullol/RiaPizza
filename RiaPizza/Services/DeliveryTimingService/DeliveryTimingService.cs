using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.DeliveryTimingService
{
    public class DeliveryTimingService : IDeliveryTimingService
    {
        private readonly AppDbContext _context;
        public DeliveryTimingService(AppDbContext context)
        {
            _context = context;
        }
        //Insert
        public async Task AddDeliveryTiming(DeliveryTiming addDeliveryTiming)
        {
           await _context.DeliveryTimings.AddAsync(addDeliveryTiming);
           await _context.SaveChangesAsync();
        }
        //Delete
        public async Task DeleteDeliveryTiming(int id)
        {
            DeliveryTiming deliveryTiming =await _context.DeliveryTimings.FindAsync(id);
            _context.DeliveryTimings.Remove(deliveryTiming);
           await _context.SaveChangesAsync();
        }
        //GetList
        public async Task<List<DeliveryTiming>> GetAllTiming()
        {
            var allDeliveryTiming = await _context.DeliveryTimings.ToListAsync();
            return allDeliveryTiming;
        }

        public async Task<DeliveryTiming> GetById(int id)
        {
            var timing =await _context.DeliveryTimings.FindAsync(id);
            return timing;
        }

        //Update
        public async Task UpdateDeliveryTiming(DeliveryTiming upadteDeliveryTiming)
        {
            var updateTimng = await _context.DeliveryTimings.FindAsync(upadteDeliveryTiming.Id);
            updateTimng.Day = upadteDeliveryTiming.Day;
            updateTimng.Timings = upadteDeliveryTiming.Timings;
            _context.DeliveryTimings.Update(updateTimng);
           await _context.SaveChangesAsync();
        }
    }
}
