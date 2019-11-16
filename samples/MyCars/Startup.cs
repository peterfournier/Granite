using AutoMapper;
using GraniteCore.AutoMapper;
using GraniteCore.EntityFrameworkCore;
using GraniteCore.MVC.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCars.Areas.Identity;
using MyCars.Data;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;
using MyCars.Domain.ViewModels;
using MyCars.Services;
using GraniteCore.RavenDB;
using Raven.Client.Documents;

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


            services.AddDefaultIdentity<GraniteCoreApplicationUser>() // GraniteCore install
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // GraniteCore install
            services.AddGraniteEntityFrameworkCore();
            //services.AddGraniteRavenDB(() => new DocumentStore()
            //{
            //    Urls = new[]
            //    {
            //        Configuration.GetConnectionString("RavenDB")
            //    },
            //    Database = "GraniteCoreSandbox",
            //    Conventions = { }
            //});
            services.AddGraniteAutoMapper(config =>
            {
                config.CreateCarMapping();
                config.CreateCustomerMapping();
                config.CreateUserMapping();
            });

            services.AddScoped(typeof(DbContext), typeof(ApplicationDbContext));
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICustomerService, CustomerService>();
            // Use Mock
            //services.AddSingleton(typeof(IBaseRepository<,,,>), typeof(MockRepository<,,,>));
            // testing commits...
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
                    .ForMember(x => x.ID, x => x.Ignore())
                    .ReverseMap()
                    ;
        }

        public static void CreateCustomerMapping(this IMapperConfigurationExpression config)
        {
            config.CreateMap<CustomerViewModel, CustomerDTO>()
                    .ReverseMap()
                    ;

            config.CreateMap<CustomerDTO, CustomerEntity>()
                    .ReverseMap()
                    ;
        }

        public static void CreateUserMapping(this IMapperConfigurationExpression config)
        {
            config.CreateMap<GraniteCoreApplicationUser, UserViewModel>()
                    .ReverseMap()
                    ;
        }
    }
}
