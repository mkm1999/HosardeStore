using Application.Categories.AddCategoryService;
using Application.Categories.DeleteCategoryService;
using Application.Categories.EditCategoryService;
using Application.Categories.GetCategoriesService;
using Application.Categories.GetChildsCategories;
using Application.ImageUploadService;
using Application.Interfaces.Context;
using Application.PrivacyAndPolicy.IGetPrivacyAndPolicyService;
using Application.PrivacyAndPolicy.SetPrivacyAndPolicy;
using Application.Products.AddNewProductService;
using Application.Products.DeleteProductService;
using Application.Products.FindProductService;
using Application.Products.GetProductsForAdminService;
using Domain.Entities;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UI
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
            string ConnectionString = Configuration["ConnectionStrings:SqlServer"];
            services.AddDbContext<DataBaseContext>(p => p.UseSqlServer(ConnectionString));

            services.AddControllersWithViews();

            services.AddIdentity<User,IdentityRole<int>>()
                .AddEntityFrameworkStores<DataBaseContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole<int>>()
                .AddErrorDescriber<CustomIdentityError>();

            services.Configure<IdentityOptions>(option =>
            {
                //UserSetting
                //option.User.AllowedUserNameCharacters = "abcd123";
                option.User.RequireUniqueEmail = true;

                //Password Setting
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;//!@#$%^&*()_+
                option.Password.RequireUppercase = false;
                option.Password.RequiredLength = 8;
                option.Password.RequiredUniqueChars = 1;

                //Lokout Setting
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                //SignIn Setting
                option.SignIn.RequireConfirmedAccount = false;
                option.SignIn.RequireConfirmedEmail = false;
                option.SignIn.RequireConfirmedPhoneNumber = true;

            });

            services.ConfigureApplicationCookie(option =>
            {
                // cookie setting
                option.ExpireTimeSpan = TimeSpan.FromDays(5);

                option.LoginPath = "/authentication/login";
                option.AccessDeniedPath = "/authentication/AccessDenied";
                option.SlidingExpiration = true;
            });
            services.AddScoped<IDataBaseContext, DataBaseContext>();

            //Policy Services
            services.AddTransient<IGetPolicy, GetPolicy>();
            services.AddTransient<ISetPrivacy, SetPrivacy>();
            //Category Services
            services.AddTransient<IAddCategory, AddCategory>();
            services.AddTransient<IGetCategories, GetCategories>();
            services.AddTransient<IDeleteCategory, DeleteCategory>();
            services.AddTransient<IEditCategory, EditCategory>();
            services.AddTransient<IGetChildCategories, GetChildCategories>();

            services.AddTransient<IUploadImage, UploadImage>();

            //Products Services
            services.AddTransient<IAddNewProduct,AddNewProduct>();
            services.AddTransient<IGetProductsForAdmin,GetProductsForAdmin>();
            services.AddTransient<IDeleteProduct, DeleteProduct>();
            services.AddTransient<IFindProduct, FindProduct>();


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
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
