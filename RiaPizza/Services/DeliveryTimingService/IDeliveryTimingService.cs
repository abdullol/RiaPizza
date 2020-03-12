using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.DeliveryTimingService
{
     public interface IDeliveryTimingService
    {
        Task AddDeliveryTiming(DeliveryTiming addDeliveryTiming);
        Task UpdateDeliveryTiming(DeliveryTiming upadteDeliveryTiming);
        Task<List<DeliveryTiming>> GetAllTiming();
        Task DeleteDeliveryTiming(int id);
        Task<DeliveryTiming> GetById(int id);
    }
}
