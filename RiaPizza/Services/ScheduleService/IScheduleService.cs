using Microsoft.AspNetCore.Http;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.ScheduleService
{
    public interface IScheduleService
    {
        Task AddorUpdate(ShopSchedule timings);
        bool isShopOpen();
        Task DeleteSchedule();
        Task ToggleShop(string status);
        ShopSchedule GetSchedule();
    }
}
