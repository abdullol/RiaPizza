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
            Coupon coupon = await _context.Coupons.FindAsync(editCoupon.Id);
           _context.Coupons.Update(coupon);
          await  _context.SaveChangesAsync();
        }

        public async Task<List<Coupon>> GetAllCoupons()
        {
            var allCoupons = await _context.Coupons.ToListAsync();
            return allCoupons;
        }

        public async Task<Coupon> ValidateCoupon(string code)
        {
            var coupon = await _context.Coupons.SingleOrDefaultAsync(s => s.Code == code);
            return coupon;
        }
    }
}
