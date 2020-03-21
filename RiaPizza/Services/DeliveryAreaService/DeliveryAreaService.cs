using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace RiaPizza.Services.DeliveryAreaService
{
    public class DeliveryAreaService : IDeliveryAreaService
    {
        private readonly AppDbContext _context;
        public DeliveryAreaService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddDeliveryArea(DeliveryArea addDeliveryArea)
        {
            addDeliveryArea.Status = true;
            addDeliveryArea.IsDeliveryAvailable = true;

            await _context.DeliveryAreas.AddAsync(addDeliveryArea);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DeliveryArea>> AllDeliveryAreas()
        {
            var allAreas = await _context.DeliveryAreas.ToListAsync();
            return allAreas;
        }

        public async Task ChangeAreaStatus(int id)
        {
            DeliveryArea deliveryArea=await _context.DeliveryAreas.FindAsync(id);
            if (deliveryArea.Status==true)
            {
                deliveryArea.Status = false;
                _context.DeliveryAreas.Update(deliveryArea);
                _context.SaveChanges();
            }
            else if (deliveryArea.Status == false) 
            {
                deliveryArea.Status = true;
                _context.DeliveryAreas.Update(deliveryArea);
                _context.SaveChanges();
            }

        }

        public async Task Delete(int id)
        {
           DeliveryArea deliveryArea=await _context.DeliveryAreas.FindAsync(id);
            _context.DeliveryAreas.Remove(deliveryArea);
           await _context.SaveChangesAsync();

        }

        public async Task EditDeliveryArea(DeliveryArea editDeliveryArea)
        {
            var deliveryArea = await _context.DeliveryAreas.FindAsync(editDeliveryArea.DeliveryAreaId);
            deliveryArea.City = editDeliveryArea.City;
            deliveryArea.AreaName = editDeliveryArea.AreaName;
            deliveryArea.PostalCode = editDeliveryArea.PostalCode;
            deliveryArea.DeliveryCharges = editDeliveryArea.DeliveryCharges;
            deliveryArea.MinOrderCharges = editDeliveryArea.MinOrderCharges;

            _context.DeliveryAreas.Update(deliveryArea);
            await _context.SaveChangesAsync();
        }

        public async Task<DeliveryArea> GetDeliveryArea(string area)
        {
            var _area = await _context.DeliveryAreas.SingleOrDefaultAsync(s => s.AreaName == area);
            return _area;
        }
        
        public async Task ToggleDeliveryService(int id, bool isAvailable)
        {
            var deliveryArea = await _context.DeliveryAreas.FindAsync(id);
            deliveryArea.IsDeliveryAvailable = isAvailable;

            _context.DeliveryAreas.Update(deliveryArea);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AreaOtherThanThis(string area, int id)
        {
            var exist = await _context.DeliveryAreas.AnyAsync(s => s.AreaName == area);
            if (exist)
            {
                var _area = await _context.DeliveryAreas.SingleOrDefaultAsync(s => s.AreaName == area);
                if (_area.DeliveryAreaId == id)
                    return false;
                else
                    return true;
            }
            return false;
        }

        public Task<bool> ValidateArea(string area)
        {
            var exist = _context.DeliveryAreas.AnyAsync(s => s.AreaName == area);
            return exist;
        }

    }
}
