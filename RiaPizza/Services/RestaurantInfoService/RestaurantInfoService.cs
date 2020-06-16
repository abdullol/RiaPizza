using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.RestaurantInfoService
{
    public class RestaurantInfoService : IRestaurantInfoService
    {
        public readonly AppDbContext _context;
        public RestaurantInfoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLocation(RestaurantInfo model)
        {
            var value = await _context.RestaurantInfos.FirstOrDefaultAsync();
            if (value != null)
            {
                value.RestaurantLocation = model.RestaurantLocation;
                _context.RestaurantInfos.Update(value);
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.RestaurantInfos.AddAsync(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddOwnerDetails(RestaurantInfo model)
        {
            var value = await _context.RestaurantInfos.FirstOrDefaultAsync();
            if (value != null)
            {
                value.OwnerDetails = model.OwnerDetails;
                _context.RestaurantInfos.Update(value);
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.RestaurantInfos.AddAsync(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RestaurantInfo> GetLocation()
        {
            var value = await _context.RestaurantInfos.FirstOrDefaultAsync();
            return value;
        }
    }
}
