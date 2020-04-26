using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Data.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        public AccountService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAddress(AppUserAddress address)
        {
            await _context.AppUserAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddress(int id)
        {
            var address = await _context.AppUserAddresses.FindAsync(id);
            _context.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AppUserAddress>> GetAddresses(int id)
        {
            var addresses = await _context.AppUserAddresses.Where(s => s.UserId == id).ToListAsync();
            return addresses;
        }

        public async Task<List<AppUserAddress>> GetAddressesWithPostalCode(int id, string code)
        {
            var addresses = await _context.AppUserAddresses.Where(s => s.UserId == id && s.PostalCode == code).ToListAsync();
            return addresses;
        }

        public async Task UpdateAddress(AppUserAddress address)
        {
            var userAddress = await _context.AppUserAddresses.FindAsync(address.AppUserAddressId);
            userAddress.Address = address.Address;
            userAddress.City = address.City;
            userAddress.Floor = address.Floor;
            userAddress.PostalCode = address.PostalCode;

            _context.Update(userAddress);
            await _context.SaveChangesAsync();

        }
    }
}
