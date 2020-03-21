using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.CouponService
{
    public class CouponService : ICouponService
    {
        private readonly AppDbContext _context;
        public CouponService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddCoupon(Coupon addCoupon)
        {
            addCoupon.Status = true;
            addCoupon.IsExpired = false;
            await _context.Coupons.AddAsync(addCoupon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCoupon(int id)
        {
            Coupon coupon = await _context.Coupons.FindAsync(id);
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task EditCoupon(Coupon editCoupon)
        {
            var coupon = await _context.Coupons.FindAsync(editCoupon.Id);
            coupon.Name = editCoupon.Name;
            coupon.Code = editCoupon.Code;
            coupon.DiscountPercent = editCoupon.DiscountPercent;
            coupon.ValidityFrom = editCoupon.ValidityFrom;
            coupon.ValidityTo = editCoupon.ValidityTo; 
           _context.Coupons.Update(coupon);
          await  _context.SaveChangesAsync();
        }

        public async Task<List<Coupon>> GetAllCoupons()
        {
            var allCoupons = await _context.Coupons.ToListAsync();
            return allCoupons;
        }

        public async Task<Coupon> GetById(int id)
        {
          var coupon= await _context.Coupons.FindAsync(id);
            return coupon;
        }
    }
}
