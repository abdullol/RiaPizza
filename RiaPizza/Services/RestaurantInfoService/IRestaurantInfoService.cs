using Microsoft.AspNetCore.Http;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.RestaurantInfoService
{
    public interface IRestaurantInfoService
    {
        Task AddLocation(RestaurantInfo model);
        Task AddOwnerDetails(RestaurantInfo model);
        Task<RestaurantInfo> GetLocation();
    }
}
