using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.DeliveryAreaService
{
    public interface IDeliveryAreaService
    {
        Task<bool> ValidatePostalCode(string postalCode);
        Task<DeliveryArea> GetDeliveryArea(string postalCode);
        Task AddDeliveryArea(DeliveryArea addDeliveryArea);
        Task EditDeliveryArea(DeliveryArea editDeliveryArea);
        Task ToggleDeliveryService(int id, bool isAvailable);
        Task<List<DeliveryArea>> AllDeliveryAreas();
        Task<bool> PostalCodeOtherThanThis(string postalCode, int id);
        Task Delete(int id);
        Task ChangeAreaStatus(int id);
    }
}
