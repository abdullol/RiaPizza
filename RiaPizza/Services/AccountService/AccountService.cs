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
    }
}
