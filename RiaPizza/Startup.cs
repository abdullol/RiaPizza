using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiaPizza.Data;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.Services.AccountService;
using RiaPizza.Services.CouponService;
using RiaPizza.Services.DeliveryAreaService;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.DishService;
using RiaPizza.Services.NotifyOrder;
using RiaPizza.Services.OrderService;
using RiaPizza.Services.RenderViewService;
using RiaPizza.Services.ScheduleService;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;

namespace RiaPizza
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.ConfigureApplicationCookie(
                options => options.LoginPath = "/Auth/Login"
            );
            services.ConfigureApplicationCookie(
               options => options.AccessDeniedPath = "/Access/Denied"
            );
            services.AddTransient<IDeliveryAreaService, DeliveryAreaService>();
            services.AddTransient<IDishCategoryService, DishCategoryService>();
            services.AddTransient<IDishService, DishService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<ICouponService, CouponService>();
            //services.AddTransient<ICustomizeThemeService, CustomizeThemeService>();


            services.AddHangfire(s => s.UseSqlServerStorage(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddSignalR();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseHangfireServer();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotifyHub>("/NotifyHub");
            });

            CreateRoles(serviceProvider);

        }


        private void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            Task<IdentityResult> roleResult;
            string name = "demo";

            //Check that there is an Administrator role and create if not
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
            hasAdminRole.Wait();

            //if (!hasAdminRole.Result)
            //{
            //    roleResult = roleManager.CreateAsync(new AppRole("Admin"));
            //    roleResult.Wait();
            //}

            //Check if the user exists and create it if not
            Task<AppUser> testUser = userManager.FindByNameAsync(name);
            testUser.Wait();

            if (testUser.Result == null)
            {
                AppUser administrator = new AppUser();
                administrator.Email = name + "@gmail.com";
                administrator.UserName = name;

                //giving username and password
                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "1234Demo..");
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                    newUserRole.Wait();
                }
            }
        }
    }
}
