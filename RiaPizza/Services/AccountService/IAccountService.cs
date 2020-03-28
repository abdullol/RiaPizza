using RiaPizza.Data.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.AccountService
{
    public interface IAccountService
    {
        Task AddUserAddress(AppUserAddress address);
        Task<List<AppUserAddress>> GetAddresses(int id);
        Task<List<AppUserAddress>> GetAddressesWithPostalCode(int id,string code);
    }
}
