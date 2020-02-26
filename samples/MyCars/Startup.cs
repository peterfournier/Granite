using AutoMapper;
using GraniteCore.AutoMapper;
using GraniteCore.EntityFrameworkCore;
using GraniteCore.MVC.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCars.Areas.Identity;
using MyCars.Data;
using MyCars.Domain.DTOs;
using MyCars.Domain.Models;
using MyCars.Domain.ViewModels;
using MyCars.Services;
using Microsoft.Extensions.Hosting;
using MyCars.ServerConfigs;
using Microsoft.AspNetCore.Http;

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
            addDatabaseContext(services);

            addAspNetIdentityWithGraniteCore(services);

            services.AddControllersWithViews();
            services.AddRazorPages();

            addIdentityServer(services);

            addGraniteCore(services);
        }

        private void addCookieAuthenication(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        private void addDatabaseContext(IServiceCollection services)
        {
            services.AddDbContext<AspNetCoreIdentityDbContext>(options =>
                            options.UseSqlServer(
                                Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        private static void addAspNetIdentityWithGraniteCore(IServiceCollection services)
        {
            services.AddDefaultIdentity<GraniteCoreApplicationUser>(
                             // options => options.SignIn.RequireConfirmedAccount = false
                             )
                            .AddEntityFrameworkStores<AspNetCoreIdentityDbContext>();
        }

        private void addIdentityServer(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryClients(IdentityServerConfig.Clients)
                .AddAspNetIdentity<GraniteCoreApplicationUser>();
            ;

            builder.AddDeveloperSigningCredential();
        }

        private static void addGraniteCore(IServiceCollection services)
        {
            services.AddGraniteEntityFrameworkCore();
            // for RavenDB
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseDatabaseErrorPage(); EFCore sql server
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

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
                endpoints.MapRazorPages();
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
            config.CreateMap<ApplicationUser, UserViewModel>()
                    .ReverseMap()
                    ;
        }
    }
}
