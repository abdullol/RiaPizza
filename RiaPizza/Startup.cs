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
using RiaPizza.Services.DeliveryAreaService;
using RiaPizza.Services.DishCategoryService;
using RiaPizza.Services.DishService;
using RiaPizza.Services.NotifyOrder;
using RiaPizza.Services.OrderService;
using RiaPizza.Services.ScheduleService;

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

            //services.AddMvc(s=>s.EnableEndpointRouting = false);
            services.AddSignalR();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotifyHub>("/NotifyHub");
            });
        }
    }
}
