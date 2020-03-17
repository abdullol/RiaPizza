using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.CouponService
{
   public interface ICouponService
    {
        Task AddCoupon(Coupon addCoupon);
        Task<List<Coupon>> GetAllCoupons();
        Task EditCoupon(Coupon editCoupon);
        Task DeleteCoupon(int id);
    }
}
