using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
    AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //AppUser Addresses
        public DbSet<AppUserAddress> AppUserAddresses { get; set; }

        public DbSet<DeliveryArea> DeliveryAreas { get; set; }
        public DbSet<DishCategory> DishCategories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishExtraType> DishExtraTypes { get; set; }
        public DbSet<DishExtra> DishExtras { get; set; }
        public DbSet<DishCart> DishCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBy> OrderBy { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderDeliveryAddress> OrderDeliveryAddresses { get; set; }
        public DbSet<ShopSchedule> ShopSchedule { get; set; }
        public DbSet<DishSize> DishSize { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

    }
}
