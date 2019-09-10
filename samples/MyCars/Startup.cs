using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCars.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GraniteCore.EntityFrameworkCore;
using GraniteCore.AutoMapper;
using MyCars.Services;
using GraniteCore;
using AutoMapper;
using MyCars.Domain.DTOs;
using MyCars.Domain.ViewModels;
using MyCars.Domain.Models;

namespace MyCars
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // GraniteCore install
            services.AddGraniteEntityFrameworkCore();
            services.AddGraniteAutoMapper(config =>
            {
                config.CreateCarMapping();
            });
            services.AddScoped<ICarService, CarService>();
            // Use Mock
            services.AddSingleton(typeof(IBaseRepository<,,,>), typeof(MockRepository<,,,>));
            // Use DataBase
            //services.AddScoped(typeof(IBaseRepository<,,,>), typeof(BaseRepository<,,,>));
            // end GraniteCore install
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        
    }


    // normally this would be in different folder
    internal static class MapperExtensions
    {
        public static void CreateCarMapping(this IMapperConfigurationExpression config)
        {
            config.CreateMap<CarViewModel, CarDTO>()
                    .ReverseMap()
                    ;

            config.CreateMap<CarDTO, CarEntity>()
                    .ReverseMap()
                    ;
        }
    }
}
